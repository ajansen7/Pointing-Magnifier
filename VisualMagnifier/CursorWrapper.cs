using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public class CursorWrapper
    {
        private int count;

        public CursorWrapper()
        {
            count = 0;
        }

        public bool Visible
        {
            get { return (count >= 0); }
            set
            {
                if (value)
                {
                    while (count < 0)
                    {
                        Cursor.Show();
                        count++;
                    }
                }
                else
                {
                    while (count >= 0)
                    {
                        Cursor.Hide();
                        count--;
                    }
                }

            }
        }
    }
}
