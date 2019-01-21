using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino;
using Rhino.Geometry.Intersect;
using Rhino.Collections;


namespace EchidnaUtilities
{

    public static class utils
    {
        public static int[] GetLoopFaces(Mesh themesh, int faceselection)
            {
            int[] loopfaces = new int[] { };
            MeshFace face = themesh.Faces[faceselection];
            Rhino.Geometry.Collections.MeshTopologyEdgeList thelist = themesh.TopologyEdges;
            int[] edgesforface = thelist.GetEdgesForFace(faceselection);

            List<double> borders = new List<double>();
            foreach (int item in edgesforface)
            {
                borders.Add(thelist.EdgeLine(item).Length);
            }
            double[] borderarray = borders.ToArray();
            Array.Sort(borderarray, edgesforface);
            int maxline = edgesforface[borderarray.Length - 1];
            int minline = edgesforface[0];

            int[] min = thelist.GetConnectedFaces(minline);
            List<int> minlist = new List<int>();
            minlist.AddRange(min);
            minlist.Remove(minlist.IndexOf(faceselection));

            int[] max = thelist.GetConnectedFaces(maxline);
            List<int> maxlist = new List<int>();
            maxlist.AddRange(max);
            maxlist.Remove(maxlist.IndexOf(faceselection));
            return loopfaces;
            }

        public static bool SelfIntersect(Mesh m)
        {
            Rhino.Geometry.Collections.MeshFaceList thefacelist = m.Faces;
            int potentialclashes = Convert.ToInt32(Math.Pow(m.Faces.Count, 2));
            Rhino.IndexPair[] theclashes = thefacelist.GetClashingFacePairs(potentialclashes);
            if (theclashes.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            /*DataTree<int> clashindices = new DataTree<int>();
            int count = 0;
            foreach (Rhino.IndexPair thepairs in theclashes)
            {
                int[] temppairs = new int[] { thepairs.I, thepairs.J };
                GH_Path temppath = new GH_Path(count);
                clashindices.AddRange(temppairs, temppath);
                count++;
            }
           */
        }
        public static string CreateIntString(List<int> theints)
        {
            List<char> thechars = new List<char>();
            for (int i = 0; i < theints.Count; i++)
            {
                char[] temp = theints[i].ToString().ToCharArray();
                thechars.AddRange(temp);
                if (i != theints.Count-1)
                {
                    thechars.Add(',');
                }
            }
            char[] thearray = thechars.ToArray();
            string intstring = new string(thearray);
            return intstring;
        }
        public static List<int> ReadIntString(string IntString)
        {
            string[] postsplit = IntString.Split(',');
            List<int> values = new List<int>();
            foreach (string item in postsplit)
            {
                int temp = int.Parse(item);
                values.Add(temp);
            }
            return values;
        }
        public static List<bool> GetFaceTypes(Mesh themesh)
        {
            bool tri = false;
            bool quad = false;
            bool ngon = false;

            if (themesh.Faces.TriangleCount >= 1)
            {
                tri = true;
            }
            if (themesh.Faces.QuadCount >= 1)
            {
                quad = true;
            }
            if (themesh.Ngons.Count >= 1)
            {
                ngon = true;
            }

           

            if (ngon)
            {
                HashSet<int> TriIndices = new HashSet<int>();
                HashSet<int> QuadIndices = new HashSet<int>();
                HashSet<int> subfaces = new HashSet<int>();
                List<uint> all = new List<uint>();
                foreach (MeshNgon current in themesh.Ngons)
                {
                    uint[] subs = current.FaceIndexList();
                    all.AddRange(subs);
                    foreach (uint item in all)
                    {
                       subfaces.Add(Convert.ToInt32(item));
                    }
                }
                int count = 0;

                foreach (MeshFace face in themesh.Faces)
                {
                    bool type = face.IsQuad;
                    if (type)
                    {
                        QuadIndices.Add(count);
                    }
                    else
                    {
                        TriIndices.Add(count);
                    }
                    count++;
                }
                bool trisubset = TriIndices.IsSubsetOf(subfaces);
                bool quadsubset = QuadIndices.IsSubsetOf(subfaces);
                if (trisubset)
                {
                    tri = false;
                }
                if (quadsubset)
                {
                    quad = false;
                }
            }

            List<bool> thetypes = new List<bool>() { tri, quad, ngon };
            return thetypes;
           

        }
        public static bool IsConvex(Mesh themesh)
        {
            leopard.CleanMesh(themesh);
        
            Point3d[] thevertices = themesh.Vertices.ToPoint3dArray();
            Point3d average = PointAverage(thevertices);
            bool isconvex = false;
            for (int i = 0; i < themesh.TopologyEdges.Count; i++)
            {
                Line theline = themesh.TopologyEdges.EdgeLine(i);
                int[] adjacentindices = themesh.TopologyEdges.GetConnectedFaces(i);
                if (adjacentindices.Length <= 1)
                {
                    isconvex = false;
                    break;
                }

                Point3d a = themesh.Faces.GetFaceCenter(adjacentindices[0]);
                Point3d midpoint = theline.PointAt(0.5);
                Point3d b = themesh.Faces.GetFaceCenter(adjacentindices[1]);
                if (midpoint.DistanceTo(average) >= a.DistanceTo(average) && midpoint.DistanceTo(average) >= b.DistanceTo(average))
                {
                    isconvex = true;
                }
            }
            return isconvex;
        }
        public static Point3d PointAverage(List<Point3d> thepoints)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            int count = thepoints.Count;
            foreach (Point3d item in thepoints)
            {
                x += item.X;
                y += item.Y;
                z += item.Z;
            }
            x /= count;
            y /= count;
            z /= count;
            Point3d average = new Point3d(x, y, z);

            return average;
        }
        public static Point3d PointAverage(Point3d[] thepoints)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            int count = thepoints.Length;
            foreach (Point3d item in thepoints)
            {
                x += item.X;
                y += item.Y;
                z += item.Z;
            }
            x /= count;
            y /= count;
            z /= count;
            Point3d average = new Point3d(x, y, z);

            return average;
        }
        public static Dictionary<string, IGH_Goo> CreateSignature(Mesh themesh)
        {
            Dictionary<string, IGH_Goo> thedict = new Dictionary<string, IGH_Goo>();
            string isclosedkey = "isclosed";
            bool isclosedvalue = themesh.IsClosed;
            string facecountkey = "facecount";
            int facecountvalue = themesh.Faces.Count;
            List<bool> facetypevalue = utils.GetFaceTypes(themesh);//0,1,2 correspond to existence of triangles,quads,and ngons.
            string trikey = "tri";
            bool trivalue = facetypevalue[0];
            string quadkey = "quad";
            bool quadvalue = facetypevalue[1];
            string ngonkey = "ngon";
            bool ngonvalue = facetypevalue[2];
            string isconvexkey = "isconvex";
            bool isconvexvalue = utils.IsConvex(themesh);
            string isdisjointkey = "isdisjoint";
            bool isdisjointvalue = utils.IsDisjoint(themesh);
            string selfintersectkey = "selfintersect";
            bool selfintersectvalue = utils.SelfIntersect(themesh);
            string isvalidkey = "isvalid";
            bool isvalidvalue = themesh.IsValid;
            string isflatkey = "isflat";
            bool isflatvalue = utils.IsFlat(themesh);
         



            thedict.Add(isclosedkey, GH_Convert.ToGoo(isclosedvalue));
            thedict.Add(facecountkey, GH_Convert.ToGoo(facecountvalue));
            thedict.Add(trikey, GH_Convert.ToGoo(trivalue));
            thedict.Add(quadkey, GH_Convert.ToGoo(quadvalue));
            thedict.Add(ngonkey, GH_Convert.ToGoo(ngonvalue));
            thedict.Add(isconvexkey, GH_Convert.ToGoo(isconvexvalue));
            thedict.Add(isdisjointkey, GH_Convert.ToGoo(isdisjointvalue));
            thedict.Add(selfintersectkey, GH_Convert.ToGoo(selfintersectvalue));
            thedict.Add(isvalidkey, GH_Convert.ToGoo(isvalidvalue));
            thedict.Add(isflatkey, GH_Convert.ToGoo(isflatvalue));
            return thedict;
        }
        public static bool ReadConditions(List<IGH_Goo> TheConditions,ref List<int>ConditionInts, ref string StrategyName, ref int StrategyIndex, ref List<int> PermittedTiers, ref int RandomizationWeight)
        {
            IGH_Goo closuretypegoo = TheConditions[0];
            IGH_Goo minfacesgoo = TheConditions[1];
            IGH_Goo maxfacesgoo = TheConditions[2];
            IGH_Goo tritypegoo = TheConditions[3];
            IGH_Goo quadtypegoo = TheConditions[4];
            IGH_Goo ngontypegoo = TheConditions[5];
            IGH_Goo isconvextypegoo = TheConditions[6];
            IGH_Goo isdisjointtypegoo = TheConditions[7];
            IGH_Goo selfintersecttypegoo = TheConditions[8];
            IGH_Goo isvalidtypegoo = TheConditions[9];
            IGH_Goo isflattypegoo = TheConditions[10];
            IGH_Goo strategynamegoo = TheConditions[11];
            IGH_Goo strategyindexgoo = TheConditions[12];
            IGH_Goo permittedtiersgoo = TheConditions[13];
            IGH_Goo randomizationweightgoo = TheConditions[14];

            int closuretype = 0;
            int minfaces = 0;
            int maxfaces = 0;
            int tritype = 0;
            int quadtype = 0;
            int ngontype = 0;
            int isconvextype = 0;
            int isdisjointtype = 0;
            int selfintersecttype = 0;
            int isvalidtype = 0;
            int isflattype = 0;
            string strategyname = "";
            int strategyindex = 0;
            string xtiers = "";
            int randomizationweight = 0;

            bool convertone = GH_Convert.ToInt32_Primary(closuretypegoo, ref closuretype);
            bool converttwo = GH_Convert.ToInt32_Primary(minfacesgoo, ref minfaces);
            bool convertthree = GH_Convert.ToInt32_Primary(maxfacesgoo, ref maxfaces);
            bool convertfour = GH_Convert.ToInt32_Primary(tritypegoo, ref tritype);
            bool convertfive = GH_Convert.ToInt32_Primary(quadtypegoo, ref quadtype);
            bool convertsix = GH_Convert.ToInt32_Primary(ngontypegoo, ref ngontype);
            bool convertseven = GH_Convert.ToInt32_Primary(isconvextypegoo, ref isconvextype);
            bool converteight = GH_Convert.ToInt32_Primary(isdisjointtypegoo, ref isdisjointtype);
            bool convertnine = GH_Convert.ToInt32_Primary(selfintersecttypegoo, ref selfintersecttype);
            bool convertten = GH_Convert.ToInt32_Primary(isvalidtypegoo, ref isvalidtype);
            bool converteleven = GH_Convert.ToInt32_Primary(isflattypegoo, ref isflattype);
            bool converttwelve = GH_Convert.ToString_Primary(strategynamegoo, ref strategyname);
            bool convertthirteen = GH_Convert.ToInt32_Primary(strategyindexgoo, ref strategyindex);
            bool convertfourteen = GH_Convert.ToString_Primary(permittedtiersgoo, ref xtiers);
            bool convertfifteen = GH_Convert.ToInt32_Primary(randomizationweightgoo, ref randomizationweight);
            
            //make the list of ints that will actually be compared.
            List<int> tempcollection = new List<int> {closuretype, minfaces, maxfaces, tritype, quadtype, ngontype, isconvextype, isdisjointtype, selfintersecttype, isvalidtype, isflattype};
            ConditionInts = tempcollection;

            //read everything else
            StrategyName = strategyname;
            StrategyIndex = strategyindex;
            PermittedTiers = utils.ReadIntString(xtiers);
            RandomizationWeight = randomizationweight;

            return true;

        }
     
        public static bool ReadSignature(Dictionary<string, IGH_Goo> Signature, ref bool isclosedconverted,ref int facecountconverted, ref bool triconverted,ref bool quadconverted, ref bool ngonconverted, ref bool isconvexconverted, ref bool isdisjointconverted, ref bool selfintersectconverted, ref bool isvalidconverted, ref bool isflatconverted)
        {
            IGH_Goo isclosed;
            IGH_Goo facecount;
            IGH_Goo tri;
            IGH_Goo quad;
            IGH_Goo Ngon;
            IGH_Goo isconvex;
            IGH_Goo isdisjoint;
            IGH_Goo selfintersect;
            IGH_Goo isvalid;
            IGH_Goo isflat;


            Signature.TryGetValue("isclosed", out isclosed);
            Signature.TryGetValue("facecount", out facecount);
            Signature.TryGetValue("tri", out tri);
            Signature.TryGetValue("quad", out quad);
            Signature.TryGetValue("ngon", out Ngon);
            Signature.TryGetValue("isconvex", out isconvex);
            Signature.TryGetValue("isdisjoint", out isdisjoint);
            Signature.TryGetValue("selfintersect", out selfintersect);
            Signature.TryGetValue("isvalid", out isvalid);
            Signature.TryGetValue("isvalid", out isflat);

            /* isclosedconverted = false;
             facecountconverted = 0;
             triconverted = false;
             quadconverted = false;
             ngonconverted = false;
            isconvexconverted = false;*/


            bool convertone = GH_Convert.ToBoolean_Primary(isclosed, ref isclosedconverted);
            bool converttwo = GH_Convert.ToInt32_Primary(facecount, ref facecountconverted);
            bool convertthree = GH_Convert.ToBoolean_Primary(tri, ref triconverted);
            bool convertfour = GH_Convert.ToBoolean_Primary(quad, ref quadconverted);
            bool convertfive = GH_Convert.ToBoolean_Primary(Ngon, ref ngonconverted);
            bool convertsix = GH_Convert.ToBoolean_Primary(isconvex, ref isconvexconverted);
            bool convertseven = GH_Convert.ToBoolean_Primary(isdisjoint, ref isdisjointconverted);
            bool converteight = GH_Convert.ToBoolean_Primary(selfintersect, ref selfintersectconverted);
            bool convertnine = GH_Convert.ToBoolean_Primary(isdisjoint, ref isvalidconverted);
            bool convertten = GH_Convert.ToBoolean_Primary(isflat, ref isflatconverted);

            if (convertone && converttwo && convertthree && convertfour && convertfive && convertsix && convertseven && converteight && convertnine && convertten)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckCompatability(Dictionary<string, IGH_Goo> Target, List<IGH_Goo> Candidates, int CurrentTier, ref string StrategyName,ref int StrategyIndex)
        {
            //create list of booleans for whether or not signatures are compatible.
            bool compatibility =  true;
           
            //readTargetSignature
            bool targisclosed = false;
            int targfacecount = 0;
            bool targtri = false;
            bool targquad = false;
            bool targngon = false;
            bool targisconvex = false;
            bool targisdisjoint = false;
            bool targselfintersect = false;
            bool targisvalid = false;
            bool targisflat = false;
            utils.ReadSignature(Target, ref targisclosed, ref targfacecount, ref targtri, ref targquad, ref targngon, ref targisconvex, ref targisdisjoint, ref targselfintersect, ref targisvalid, ref targisflat );

            List<bool> targets = new List<bool>() { targisclosed, targtri, targquad, targngon, targisconvex, targisdisjoint, targselfintersect, targisvalid, targisflat };

            /*
                        int closuretype = Candidates[0];
                        int minfaces = Candidates[1];
                        int maxfaces = Candidates[2];
                        Interval Facedomain = new Interval(minfaces, maxfaces);
                        int tritype = Candidates[3];
                        int quadtype = Candidates[4];
                        int ngontype = Candidates[5];
                        int isconvextype = Candidates[6];
                        int isdisjointtype = Candidates[7];
                        int selfintersecttype = Candidates[8];
                        int isvalidtype = Candidates[9];
                        int isflattype = Candidates[10];*/


            List<int> candidates = new List<int>();
            String strategyname = "";
            int strategyindex = -1;
            List<int> permittedtiers = new List<int>();
            int randomizationweight = -1;
            utils.ReadConditions(Candidates, ref candidates, ref strategyname, ref strategyindex,ref permittedtiers,ref randomizationweight);
            Interval Facedomain = new Interval(candidates[1], candidates[2]);
            StrategyName = strategyname;
            StrategyIndex = strategyindex;

            for (int i = 0; i < candidates.Count; i++)
            {
                if (utils.IsCompatible(targets[i],candidates[i]) ==false)
                {
                    compatibility = false;
                    break;
                }
            }
            if (! Facedomain.IncludesParameter(targfacecount))
            {
                compatibility = false;
            }
            if (! permittedtiers.Contains(CurrentTier))
            {
                compatibility = false;
            }

            return compatibility;
           
        }
        public static bool IsDisjoint(Mesh m)
        {
            if (m.DisjointMeshCount >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsCompatible(bool target, int condition)
        {
            if (condition == 0 && target == false)
            {
                return true;
            }
            if (condition == 1 && target == true)
            {
                return true;
            }
            if (condition == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsFlat(Mesh m)
        {
            m.Normals.ComputeNormals();
            Rhino.Geometry.Collections.MeshVertexNormalList thenormals = m.Normals;
            Vector3d target = thenormals[0];
            int indexer = 0;
            bool isflat = true;
            Plane plane = Plane.WorldXY;
            do
            {
                if (Vector3d.VectorAngle(thenormals[indexer], target) >= Math.PI / 2)
                {
                    isflat = false;
                }
                indexer++;
            } while (isflat && indexer < thenormals.Count);
            return isflat;
        }

    }
    public static class leopard
    {
        static private Random random = new Random();

        // ===============================================================================================================================================

        static public double Distance(Point3d A, Point3d B)
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2.0) + Math.Pow(A.Y - B.Y, 2.0) + Math.Pow(A.Z - B.Z, 2.0));
        }


        static public double DistanceXY(Point3d A, Point3d B)
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2.0) + Math.Pow(A.Y - B.Y, 2.0));
        }

        static public double DistanceSquared(Point3d A, Point3d B)
        {
            return Math.Pow(A.X - B.X, 2.0) + Math.Pow(A.Y - B.Y, 2.0) + Math.Pow(A.Z - B.Z, 2.0);
        }

        static public double DistanceSquaredXY(Point3d A, Point3d B)
        {
            return Math.Pow(A.X - B.X, 2.0) + Math.Pow(A.Y - B.Y, 2.0);
        }

        static public double DistancePointLine(Point3d M, Point3d A, Point3d B)
        {
            return Vector3d.CrossProduct(M - A, M - B).Length / (A - B).Length;
        }

        static public Point3d ClosestPointOnLine(Point3d M, Point3d A, Point3d B)
        {
            double distance = DistancePointLine(M, A, B);

            Vector3d AB = B - A;
            if ((M - A) * AB < 0) AB *= -1;

            return A + AB / AB.Length * Math.Sqrt(DistanceSquared(M, A) - distance * distance);
        }

        static public Point3d ClosestPointOnLine(Point3d M, Point3d A, Point3d B, ref double distance)
        {
            distance = DistancePointLine(M, A, B);
            Vector3d AB = B - A;
            if ((M - A) * AB < 0) AB *= -1;

            return A + AB / AB.Length * Math.Sqrt(DistanceSquared(M, A) - distance * distance);
        }


        static public void ResetRandom(int seed) { random = seed < 0 ? new Random() : new Random(seed); }
        static public int GetRandomInteger() { return random.Next(); }
        static public int GetRandomInteger(int minValue, int maxValue) { return random.Next(minValue, maxValue); }
        static public int GetRandomInteger(int maxValue) { return random.Next(maxValue); }
        static public double GetRandomDouble() { return random.NextDouble(); }
        static public double GetRandomDouble(double minValue, double maxValue) { return minValue + random.NextDouble() * (maxValue - minValue); }
        static public double GetRandomDouble(double maxValue) { return random.NextDouble() * maxValue; }


        static public Point3d GetRandomPoint(double minX = 0.0, double maxX = 1.0, double minY = 0.0, double maxY = 1.0, double minZ = 0.0, double maxZ = 1.0)
        {
            double x = minX + (maxX - minX) * random.NextDouble();
            double y = minY + (maxY - minY) * random.NextDouble();
            double z = minZ + (maxZ - minZ) * random.NextDouble();

            return new Point3d(x, y, z);
        }


        static public Vector3d GetRandomUnitVector()
        {
            double phi = 2.0 * Math.PI * random.NextDouble();
            double theta = Math.Acos(2.0 * random.NextDouble() - 1.0);

            double x = Math.Sin(theta) * Math.Cos(phi);
            double y = Math.Sin(theta) * Math.Sin(phi);
            double z = Math.Cos(theta);

            return new Vector3d(x, y, z);
        }


        static public Vector3d GetRandomUnitVectorXY()
        {
            double angle = 2.0 * Math.PI * random.NextDouble();

            double x = Math.Cos(angle);
            double y = Math.Sin(angle);

            return new Vector3d(x, y, 0.0);
        }


        static public Vector3d GetRandomVector(double minLength = 0.0, double maxLength = 1.0)
        {
            return GetRandomUnitVector() * (minLength + random.NextDouble() * (maxLength - minLength));
        }


        static public Vector3d GetRandomVectorXY(double minLength = 0.0, double maxLength = 1.0)
        {
            return GetRandomUnitVectorXY() * (minLength + random.NextDouble() * (maxLength - minLength));
        }


        static public Vector3d ComputePlaneNormal(Point3d A, Point3d B, Point3d C)
        {
            Vector3d normal = Vector3d.CrossProduct(B - A, C - A);
            normal.Unitize();
            return normal;
        }


        private const double degreeToRadianFactor = Math.PI / 180.0;
        private const double radianToDegreeFactor = 180.0 / Math.PI;

        static public double ToRadian(double degree)
        {
            return degree * degreeToRadianFactor;
        }


        static public double ToDegree(double radian)
        {
            return radian * radianToDegreeFactor;
        }


        static public double AngleBetweenTwoTriangles(Point3d A, Point3d B, Point3d M, Point3d N)
        {
            return AngleBetweenTwoVectors(
                M - ClosestPointOnLine(M, A, B),
                N - ClosestPointOnLine(N, A, B)
                );
        }


        /// <summary>Compute the angle between two unit vectors. The result will always lie in the interval between 0 and PI</summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The angle between two vectors</returns>
        static public double AngleBetweenTwoUnitVectors(Vector3d a, Vector3d b)
        {
            return Math.Acos(a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }


        /// <summary>Compute the angle between two unit vectors, which can be smaller than 0 or larger than PI, based on a reference Z vector that indicates the positive rotation direction</summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <param name="z">The vector that indicates the positive rotation direction</param>
        /// <param name="positive">
        ///     <para>True: the result angle will be in the interval between 0 and 2 * PI</para>
        ///     <para>False: the result angle will be in the interval between -PI and PI</para>
        /// </param>
        /// <returns>The angle between two vectors</returns>
        static public double AngleBetweenTwoUnitVectors(Vector3d a, Vector3d b, Vector3d z, bool positive = true)
        {
            double angle = Math.Acos(a.X * b.X + a.Y * b.Y + a.Z * b.Z);

            if (z * Vector3d.CrossProduct(a, b) < 0.0)
            {
                if (positive) angle = 2.0 * Math.PI - angle;
                else angle = -angle;
            }
            return angle;
        }

      
        /// <summary>Compute the angle between two vectors. The result will always lie in the interval between 0 and PI</summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The angle between two vectors</returns>
        static public double AngleBetweenTwoVectors(Vector3d a, Vector3d b)
        {
            double temp = (a.X * b.X + a.Y * b.Y + a.Z * b.Z) / (a.Length * b.Length);
            if (temp > 1.0) temp = 1.0;
            else if (temp < -1.0) temp = -1.0;
            return Math.Acos(temp);
        }


        /// <summary>Compute the angle between two vectors, which can be smaller than 0 or larger than PI, based on a reference Z vector that indicates the positive rotation direction</summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <param name="z">The vector that indicates the positive rotation direction</param>
        /// <param name="positive">
        ///     <para>True: the result angle will be in the interval between 0 and 2 * PI</para>
        ///     <para>False: the result angle will be in the interval between -PI and PI</para>
        /// </param>
        /// <returns>The angle between two vectors</returns>
        static public double AngleBetweenTwoVectors(Vector3d a, Vector3d b, Vector3d z, bool positive = true)
        {
            double temp = (a.X * b.X + a.Y * b.Y + a.Z * b.Z) / (a.Length * b.Length);
            if (temp > 1.0) temp = 1.0;
            else if (temp < -1.0) temp = -1.0;

            double angle = Math.Acos(temp);

            if (z * Vector3d.CrossProduct(a, b) < 0.0)
            {
                if (positive) angle = 2.0 * Math.PI - angle;
                else angle = -angle;
            }

            return angle;
        }



        //////////////////////// my mesh function /////////////////////////////////
        public static void CleanMesh(Mesh m)
        {
            m.Compact();
            m.Vertices.CombineIdentical(true, true);
            m.Vertices.CullUnused();
            m.Weld(3.1415926535897931);
            m.UnifyNormals();
            m.FaceNormals.ComputeFaceNormals();
            m.Normals.ComputeNormals();
           
        }
    }
    
}
