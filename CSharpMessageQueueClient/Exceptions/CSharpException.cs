namespace CSharpMessageQueueClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CSharpException: Exception
    {
        public CSharpException() { }

        public CSharpException(string msg) : base(msg) { }
    }
}
