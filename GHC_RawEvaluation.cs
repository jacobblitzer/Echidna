using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino;
using Rhino.Geometry.Intersect;
using Rhino.Collections;
using RawUtilities;


// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace RawPlugins
{
    public class GHC_RawEvaluation : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GHC_RawEvaluation()
          : base("GHC_RawEvaluation", "GHC_RawEvaluation",
              "Functions for Raw",
              "Extra", "Raw")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "input mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Keys", "Keys", "Keys", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Values", "Values", "Values", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            List<Mesh> inputmeshes = new List<Mesh>();

            DA.GetDataList<Mesh>(0, inputmeshes);
            //Dictionary<string,dynamic> thedict = new Dictionary<string,dynamic>();
            int count = 0;
            DataTree<string> keys = new DataTree<string>();
            var values = new DataTree<IGH_Goo>();
            foreach (Mesh themesh in inputmeshes)
            {
                string isclosedkey = "isclosed";
                bool isclosedvalue = themesh.IsClosed;
                string facecountkey = "facecount";
                int facecountvalue = themesh.Faces.Count;
                string facetypekey = "facetype";
                List<bool> facetypevalue = utils.GetFaceTypes(themesh);//0,1,2 correspond to existence of triangles,quads,and ngons.
                string isconvexkey = "isconvex";
                bool isconvexvalue = utils.IsConvex(themesh);

                GH_Path current = new GH_Path(count);

                keys.Add(isclosedkey, current);
                keys.Add(facecountkey, current);
                keys.Add(facetypekey, current);
                keys.Add(isconvexkey, current);

                values.Add(GH_Convert.ToGoo(isclosedvalue), current);
                values.Add(GH_Convert.ToGoo(facecountvalue), current);
                values.Add(GH_Convert.ToGoo(facetypevalue), current);
                values.Add(GH_Convert.ToGoo(isconvexvalue), current);

                count++;
            }
            DA.SetDataTree(0, keys);
            DA.SetDataTree(1, values);
            

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dab2bd6e-9c16-45de-91bf-5c8f149144f5"); }
        }
    }
}
