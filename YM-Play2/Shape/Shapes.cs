namespace YM2
{
    public class Shapes
    {
        const int MAX_SHAPE = 64;   // 0...63

        public int n;
        public Shape[] shape_set = new Shape[MAX_SHAPE+1];
        public readonly int dummy;

        public Shapes()
        {
            int i;

            for (i = 0; i < MAX_SHAPE; i++)
            {
                shape_set[i] = new Shape();
                shape_set[i].data = new byte[256];
                shape_set[i].name = "Empty";
            }
            n = MAX_SHAPE;
            // default shape

            shape_set[n] = new Shape();
            shape_set[n].data = new byte[256];
            shape_set[n].name = "_default_";
            shape_set[n].para1 = 1;
            shape_set[n].para2 = 0;
            shape_set[n].para3 = 0;
            shape_set[n].para4 = 0;
            shape_set[n].para5 = 0;
            shape_set[n].data[0] = 0;
            shape_set[n].data[1] = 0;
            shape_set[n].data[2] = 0xE1;
            dummy = MAX_SHAPE;
        }
    }
}