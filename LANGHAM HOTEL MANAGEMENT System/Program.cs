/*
Project Name: Langham Hotel Management System
Author Name: Bibek K c
Date: 2025-04-11
Application Purpose: To manage hotel room allocation, customer details, and save/load data with exception handling and file I/O.
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Assessment2Task2
{
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; } = false;
    }

    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    class Program
    {
        public static List<Room> listOfRooms = new List<Room>();
        public static List<RoomAllocation> listOfRoomAllocations = new List<RoomAllocation>();
        public static string filePath;
        public static string studentId = "764707333"; // Updated with actual student ID
        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, $"lhms_{studentId}.txt");
            char ans;

            do
            {
                Console.Clear();
                Console.WriteLine("******** LANGHAM HOTEL MANAGEMENT SYSTEM ********");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save the Room Allocations To a File");
                Console.WriteLine("8. Show the Room Allocations From a File");
                Console.WriteLine("0. Backup File");
                Console.WriteLine("9. Exit");
                Console.Write("Enter Your Choice Number Here: ");

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            AddRooms(); break;
                        case 2:
                            DisplayRooms(); break;
                        case 3:
                            AllocateRoom(); break;
                        case 4:
                            DeAllocateRoom(); break;
                        case 5:
                            DisplayAllocations(); break;
                        case 6:
                            Console.WriteLine("Billing Feature is Under Construction and will be added soon…!!!");
                            break;
                        case 7:
                            SaveToFile(); break;
                        case 8:
                            LoadFromFile(); break;
                        case 0:
                            BackupFile(); break;
                        case 9:
                            return;
                        default:
                            Console.WriteLine("Invalid Option!");
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input format. Please enter a number.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Operation failed: {ex.Message}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found. Please save some data first.");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Access denied. You do not have permission to write to this file.");
                }
                finally
                {
                    string response;
                    do
                    {
                        Console.Write("\nWould You Like To Continue (Y/N)? ");
                        response = Console.ReadLine().Trim().ToUpper();
                    } while (response != "Y" && response != "N");

                    ans = response[0];
                }
            } while (ans == 'Y');
        }

        static void AddRooms()
        {
            try
            {
                Console.Write("Please Enter the Total Number of Rooms in the Hotel: ");
                int total = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < total; i++)
                {
                    Console.Write("Enter Room Number: ");
                    int roomNo = Convert.ToInt32(Console.ReadLine());
                    listOfRooms.Add(new Room { RoomNo = roomNo });
                }
            }
            catch (FormatException)
            {
                throw;
            }
        }

        static void DisplayRooms()
        {
            Console.WriteLine("Available Rooms:");
            foreach (var room in listOfRooms)
            {
                Console.WriteLine($"Room No: {room.RoomNo}, Allocated: {room.IsAllocated}");
            }
        }

        static void AllocateRoom()
        {
            Console.Write("Enter Room No to Allocate: ");
            int roomNo = Convert.ToInt32(Console.ReadLine());
            Room room = listOfRooms.Find(r => r.RoomNo == roomNo);
            if (room == null || room.IsAllocated)
            {
                throw new InvalidOperationException("Room not found or already allocated.");
            }
            Console.Write("Enter Customer No: ");
            int custNo = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Customer Name: ");
            string custName = Console.ReadLine();
            room.IsAllocated = true;
            listOfRoomAllocations.Add(new RoomAllocation
            {
                AllocatedRoomNo = roomNo,
                AllocatedCustomer = new Customer { CustomerNo = custNo, CustomerName = custName }
            });
        }

        static void DeAllocateRoom()
        {
            Console.Write("Enter Room No to Deallocate: ");
            int roomNo = Convert.ToInt32(Console.ReadLine());
            Room room = listOfRooms.Find(r => r.RoomNo == roomNo);
            if (room == null || !room.IsAllocated)
            {
                throw new InvalidOperationException("Room not found or already deallocated.");
            }
            room.IsAllocated = false;
            listOfRoomAllocations.RemoveAll(a => a.AllocatedRoomNo == roomNo);
        }

        static void DisplayAllocations()
        {
            Console.WriteLine("Room Allocation Details:");
            foreach (var allocation in listOfRoomAllocations)
            {
                Console.WriteLine($"Room {allocation.AllocatedRoomNo} allocated to {allocation.AllocatedCustomer.CustomerName} (ID: {allocation.AllocatedCustomer.CustomerNo})");
            }
        }

        static void SaveToFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("\nRoom Allocation Record: " + DateTime.Now);
                foreach (var a in listOfRoomAllocations)
                {
                    sw.WriteLine($"Room: {a.AllocatedRoomNo}, Customer No: {a.AllocatedCustomer.CustomerNo}, Name: {a.AllocatedCustomer.CustomerName}");
                }
            }
            Console.WriteLine("Data Saved to File.");
        }

        static void LoadFromFile()
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            string content = File.ReadAllText(filePath);
            Console.WriteLine("File Contents:");
            Console.WriteLine(content);
        }

        static void BackupFile()
        {
            string backupFile = Path.Combine(Path.GetDirectoryName(filePath), $"lhms_{studentId}_backup.txt");
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            File.AppendAllText(backupFile, File.ReadAllText(filePath));
            File.WriteAllText(filePath, "");
            Console.WriteLine("Backup Completed and Original File Cleared.");
        }
    }
}
