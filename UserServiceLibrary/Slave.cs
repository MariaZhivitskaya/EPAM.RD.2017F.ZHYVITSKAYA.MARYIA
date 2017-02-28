using System;
using System.Collections.Generic;
using System.Linq;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates slave's service
    /// </summary>
    public class Slave
    {
        private readonly List<User> _users = new List<User>();

        /// <summary>
        /// Subscribes a slave for a master
        /// </summary>
        /// <param name="master">A master</param>
        public void Subscribe(Master master)
        {
            master.AddUserEvent += AddUser;
            master.RemoveUserEvent += RemoveUser;
        }

        /// <summary>
        /// Searches for a user in the service
        /// </summary>
        /// <param name="predicate">A search criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Null criterion!");

            return _users.Where(u => predicate(u)).ToList().ToList();
        }

        private void AddUser(User user)
        {
            _users.Add(user);
            Console.WriteLine("Slave is adding");
        }

        private void RemoveUser(User user)
        {
            _users.Remove(user);
            Console.WriteLine("Slave is removing");
        }
    }
}
