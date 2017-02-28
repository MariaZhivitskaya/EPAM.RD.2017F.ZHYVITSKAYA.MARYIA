using System;
using System.Collections.Generic;
using UserServiceLibrary;

namespace ServiceApplication
{
    class Program
    {
        public static int StartId = 2000;
        public static int SlavesNumber;

        static Program()
        {
            SlavesNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["slaves"]);
        }

        static void Main(string[] args)
        {
            try
            {
                User user1 = new User
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1980, 12, 8)
                };
                User user2 = new User
                {
                    FirstName = "Alice",
                    LastName = "Cooper",
                    DateOfBirth = new DateTime(1960, 5, 14)
                };
                User user3 = new User
                {
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1991, 5, 28)
                };
                User user4 = new User
                {
                    FirstName = "John",
                    LastName = "Lennon",
                    DateOfBirth = new DateTime(1980, 12, 8)
                };

                UserService userService = new UserService(IdGenerator);
                Master master = new Master(userService);
                MasterDomain();
                List<Slave> slaves = new List<Slave>();

                for (int i = 0; i < SlavesNumber; i++)
                {
                    slaves.Add(new Slave());
                    slaves[i].Subscribe(master);
                    SlaveDomain("SlaveDomain" + i);
                }

                master.Add(user1);
                master.Add(user2);
                master.Add(user3);
                master.Add(user4);

                master.Remove(user4);

                //var serviceMaster = new UserService(IdGenerator);
                //var service2 = new UserService(IdGenerator);

                //service1.Add(user1);
                //service2.Add(user2);
                //service1.Add(user3);
                //service2.Add(user4);

                //service.Remove(user4);

                //Predicate<User> predicate1 = u => u.LastName == user1.LastName;
                //var search1 = service.Search(predicate1);

                //Predicate<User> predicate2 = u => u.FirstName == user1.FirstName;
                //var search2 = service.Search(predicate2);

                //service.Serialize();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static int IdGenerator()
        {
            return StartId++;
        }

        private static void MasterDomain()
        {
            AppDomain masterDomain = AppDomain.CreateDomain("MasterDomain");
            //masterDomain.AssemblyLoad += Domain_
            // Create a domain with name Master.
            //var appDomainSetup = new AppDomainSetup
            //{
            //    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
            //    PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Master")
            //};
            //AppDomain domain = AppDomain.CreateDomain("Master", null, appDomainSetup);
            //var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);

            //try
            //{
            //    Result result = null;
            //    result = loader.Load("MyLibrary, Version=1.2.3.4, Culture=neutral, PublicKeyToken=f46a87b3d9a80705", input);

            //    Console.WriteLine("Method1: {0}", result.Value);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception: {0}", e.Message);
            //}

            //////
            ////AppDomain.Unload(domain);
        }

        public static void SlaveDomain(string friendlyName)
        {
            AppDomain slaveDomain = AppDomain.CreateDomain(friendlyName);
        }
    }
}
