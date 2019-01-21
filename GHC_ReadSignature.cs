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
using EchidnaUtilities;

namespace Echidnalugins
{
    public class GHC_ReadSignature : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_ReadSignature class.
        /// </summary>
        public GHC_ReadSignature()
          : base("GHC_ReadSignature", "ReadSig",
              "ReadSignature",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Signature", "Sig", "input signature", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("IsClosed", "IsClosed", "IsClosed", GH_ParamAccess.item);
            pManager.AddIntegerParameter("FaceCount", "FaceCount", "FaceCount", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Tri", "Tri", "Tri", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Quad", "Quad", "Quad", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Ngon", "Ngon", "Ngon", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsConvex", "IsConvex", "IsConvex", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsDisjoint", "IsDisjoint", "IsDisjoint", GH_ParamAccess.item);
            pManager.AddBooleanParameter("SelfIntersect", "SelfIntersect", "SelfIntersect", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsValid", "IsValid", "IsValid", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsFlat", "IsFlat", "IsFlat", GH_ParamAccess.item);


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, IGH_Goo> thedictionary = new Dictionary<string, IGH_Goo>();
            DA.GetData<Dictionary<string,IGH_Goo>>(0, ref thedictionary);
            bool isclosedconverted= false;
            int facecountconverted = 0;
            bool triconverted = false;
            bool quadconverted= false;
            bool ngonconverted = false;
            bool isconvexconverted= false;
            bool isdisjointconverted = false;
            bool selfintersectconverted = false;
            bool isvalidconverted = false;
            bool isflatconverted = false;


            utils.ReadSignature(thedictionary, ref isclosedconverted, ref facecountconverted, ref triconverted, ref quadconverted, ref ngonconverted, ref isconvexconverted, ref isdisjointconverted,ref selfintersectconverted,ref isvalidconverted,ref isflatconverted);
            DA.SetData(0, isclosedconverted);
            DA.SetData(1, facecountconverted);
            DA.SetData(2, triconverted);
            DA.SetData(3, quadconverted);
            DA.SetData(4, ngonconverted);
            DA.SetData(5, isconvexconverted);
            DA.SetData(6, isdisjointconverted);
            DA.SetData(7, selfintersectconverted);
            DA.SetData(8, isvalidconverted);
            DA.SetData(9, isflatconverted);
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
            get { return new Guid("51b94c52-3158-47ed-994b-0d91e402990d"); }
        }
    }
}