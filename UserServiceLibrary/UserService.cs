using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates a service for users
    /// </summary>
    public class UserService
    {
        private readonly Func<int> _idAlgorithm;
        private readonly List<User> _users = new List<User>();

        private static readonly string FileName;
        private static readonly bool Logging;

        static UserService()
        {
            FileName = System.Configuration.ConfigurationManager.AppSettings["fileName"];
            Logging = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["logging"]);

            if (Logging)
            {
                ConsoleTraceListener listener = new ConsoleTraceListener();
                Trace.Listeners.Add(listener);
            }
        }

        /// <summary>
        /// Initializes a new instance of the UserService class
        /// </summary>
        /// <param name="idAlgorithm">An algorithm for id generating</param>
        public UserService(Func<int> idAlgorithm)
        {
            _idAlgorithm = idAlgorithm;
        }

        /// <summary>
        /// Adds a new user to the collection
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the user is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the user is already in database
        /// </exception>
        /// <param name="user">A user</param>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Null user!");

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || user.DateOfBirth == null)
                throw new ArgumentNullException("Null user's data!");

            if (_users.Exists(curUser => curUser.FirstName == user.FirstName &&
                                        curUser.LastName == user.LastName &&
                                        curUser.DateOfBirth == user.DateOfBirth))
                throw new ArgumentException("Duplicate user!");

            user.Id = _idAlgorithm();
            _users.Add(user);
        }

        /// <summary>
        /// Removes a specified user from the collection
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the user is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the user isn't in database
        /// </exception>
        /// <param name="user">A user</param>
        public void Remove(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            if (!_users.Exists(curUser => curUser.FirstName == user.FirstName &&
                                        curUser.LastName == user.LastName &&
                                        curUser.DateOfBirth == user.DateOfBirth))
                throw new ArgumentException("No such user in database!");

            _users.Remove(user);
        }

        /// <summary>
        /// Searches for users according to a specified criterion
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the criterion is null
        /// </exception>
        /// <param name="predicate">A criterion</param>
        /// <returns>Returns a list of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Null criterion!");

            IEnumerable<User> result = _users.Where(u => predicate(u));

            if (!result.Any())
                throw new ArgumentNullException("No such user!");

            return result.ToList();
        }

        /// <summary>
        /// Serializes users from the service to a file with a specified FileName
        /// </summary>
        public void Serialize()
        {
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Serializing info in " + FileName + "...");
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(User));

            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                foreach (var user in _users)
                    xmlSerializer.Serialize(fs, user);

            if (Logging)
            {
                Trace.WriteLine("Successfully serialized!");
                Trace.WriteLine("-----------------------------\n");
            }
        }
    }
}
