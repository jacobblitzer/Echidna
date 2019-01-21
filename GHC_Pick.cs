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
    public class GHC_Pick : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_Pick class.
        /// </summary>
        public GHC_Pick()
          : base("GHC_Pick", "GHC_Pick",
              "GHC_Pick",
              "Extra", "Raw")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Strategies", "Strategies", "Strategies", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Indices", "Indices", "Indices", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("PickName", "PickName", "PickName", GH_ParamAccess.item);
            pManager.AddIntegerParameter("PickIndex", "PickIndex", "PickIndex", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> optionnames = new List<string>();
            DA.GetDataList(0, optionnames);
            List<int> optionindices = new List<int>();
            DA.GetDataList(1, optionindices);

            int optioncount = optionindices.Count;
            int random = leopard.GetRandomInteger(optioncount);
            int selected = optionindices[random];
            string selectedname = optionnames[random];

            DA.SetData(0, selectedname);
            DA.SetData(1, selected);

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
            get { return new Guid("8819e060-2ac8-49da-8ce8-21999111bb10"); }
        }
    }
}