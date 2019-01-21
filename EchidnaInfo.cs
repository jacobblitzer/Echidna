using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Echidna
{
    public class EchidnaInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Echidna";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("94b7d2d2-bea6-46cf-acb4-4a49ee2140a3");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
