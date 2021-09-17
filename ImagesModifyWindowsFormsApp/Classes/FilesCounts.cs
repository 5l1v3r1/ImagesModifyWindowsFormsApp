using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImagesModifyWindowsFormsApp.Classes
{
    public class FilesCounts
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int f;

        public int FileCount
        {
            get { return f; }
            set { f = value; }
        }

        private int i;

        public int Index_i
        {
            get { return i; }
            set { i = value; }
        }


        private bool isData;

        public bool IsData
        {
            get { return isData; }
            set { isData = value; }
        }

    }
}