using System;
using System.Collections.Generic;
using System.Linq;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates master's service
    /// </summary>
    public class Master
    {
        private readonly UserService _userService;

        /// <summary>
        /// A delegate for slaves' notifications
        /// </summary>
        /// <param name="user">A user</param>
        public delegate void MasterEventHandler(User user);

        /// <summary>
        /// An event for adding user
        /// </summary>
        public event MasterEventHandler AddUserEvent;

        /// <summary>
        /// An event for deleting user
        /// </summary>
        public event MasterEventHandler RemoveUserEvent;

        /// <summary>
        /// Initializes a new instance of the Master class 
        /// </summary>
        /// <param name="userService">An instance of the UserService class</param>
        public Master(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Adds a new user to the service
        /// </summary>
        /// <param name="user">A user</param>
        public void Add(User user)
        {
            _userService.Add(user);
            AddUserEvent?.Invoke(user);
        }

        /// <summary>
        /// Removes a user from the service
        /// </summary>
        /// <param name="user">A user</param>
        public void Remove(User user)
        {
            _userService.Remove(user);
            RemoveUserEvent?.Invoke(user);
        }

        /// <summary>
        /// Searches for a user in the service
        /// </summary>
        /// <param name="predicate">A search criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate) => 
            _userService.Search(predicate).ToList();
    }
}
