using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCharacterManagment
{
    public class FOVData
    {
        public int X_coord { get; set; }
        public int Y_coord { get; set; }
        public FOVData(int x_coord, int y_coord) {
            X_coord = x_coord;
            Y_coord = y_coord;
        }
    }
}
