using System;

namespace CommonLibrary.ViewModel
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsViewable: Attribute
    {
        public bool Value { get; set; }
    }
}