using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoEFCosmos
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DemoContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var children = new HashSet<Child>()
                {
                    new Child() { ChildName = "Name1 " + DateTime.Now },
                    new Child() { ChildName = "Name2 " + DateTime.Now }
                };


                var addParent = new Parent()
                {
                    Id = Guid.NewGuid(),
                    Singleton = new Singleton() { SingletonName = "Name " + DateTime.Now },
                    Children = children
                };

                context.Parent?.Add(addParent);
                context.SaveChanges();
                Debug.WriteLine("Count Before: " + addParent.Children?.Count);
                Debug.WriteLine("Count Before: " + children.Count);

                // Reading the parent seems to double up the children 
                var dummy = context.Parent.ToList();
                Debug.WriteLine("Count Middle: " + addParent.Children?.Count);
                Debug.WriteLine("Count Middle: " + children.Count);
                var dummy2 = context.Parent.ToList();
                Debug.WriteLine("Count After: " + addParent.Children?.Count);
                Debug.WriteLine("Count After: " + children.Count);
            }
        }
    }
}
