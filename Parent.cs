using System;
using System.Collections.Generic;

namespace DemoEFCosmos
{
    public class Parent
    {
        public Guid Id { get; set; }
        public Singleton? Singleton { get; set; }
        public ICollection<Child>? Children { get; set; }
    }
}
