using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino;
using Rhino.Geometry.Intersect;
using Rhino.Collections;
using EchidnaUtilities;

namespace Echidna
{
    public class GHC_FaceSelection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_FaceSelection class.
        /// </summary>
        public GHC_FaceSelection()
          : base("GHC_FaceSelection", "GHC_FaceSelection",
              "GHC_FaceSelection",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh", GH_ParamAccess.item);
            pManager.AddIntegerParameter("FaceIndex", "FaceIndex", "FaceIndex", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("minmax", "minmax", "minmax", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh themesh = new Mesh();
            DA.GetData(0, ref themesh);
            int faceselection = 0;
            DA.GetData(1, ref faceselection);
            DA.SetDataList(0,utils.GetLoopFaces(themesh, faceselection));

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ce6beb2d-12d2-43d3-8c3d-0c1927f0b96b"); }
        }
    }
}