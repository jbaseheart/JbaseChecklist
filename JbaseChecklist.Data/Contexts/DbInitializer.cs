using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JbaseChecklist.Domain.Models;

namespace JbaseChecklist.Data.Contexts
{
    public static class DbInitializer
    {
        public static void Initialize(SqlChecklistContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
                return;

            if (context.Users.Count() == 0)
            {
                var defaultUser = new User() { Username = "jbase" };
                context.Users.Add(defaultUser);
                context.SaveChanges();

                var list1 = new Checklist()
                {
                    Name = "Default Checklist",
                    Description = "Standard checklist of todo items",
                    UserId = defaultUser.Id
                };
                context.CheckLists.Add(list1);
                context.SaveChanges();

                var list2 = new Checklist()
                {
                    Name = "Additional Checklist",
                    Description = "Second checklist of todo items",
                    UserId = defaultUser.Id
                };
                context.CheckLists.Add(list2);
                context.SaveChanges();

                context.CheckListItems.Add(new ChecklistItem
                { 
                    Description = "Item1",
                    ChecklistId = list1.Id,
                    IsComplete = false
                });
                context.SaveChanges();

            }

        }
    }
}
