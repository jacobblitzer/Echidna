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


namespace Echidna
{
    public class GHC_CreateSignature : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_RawEvaluation2 class.
        /// </summary>
        public GHC_CreateSignature()
          : base("GHC_CreateSignature", "CreateSignature",
              "CreateSignature",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "input mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Signature", "Sig", "MeshSignature", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh inputmeshes = new Mesh();

            DA.GetData<Mesh>(0, ref inputmeshes);
            Dictionary<string, IGH_Goo> thedict = utils.CreateSignature(inputmeshes);
               
            DA.SetData(0, thedict);
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
            get { return new Guid("884c3bf9-2dcb-4b8f-9513-4d96082a0c80"); }
        }
    }
}