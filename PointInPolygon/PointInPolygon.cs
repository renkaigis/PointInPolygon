using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.AnalysisTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geometry;

namespace PointInPolygon
{
    public partial class Topology : Form
    {
        public Topology()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);

            InitializeComponent();

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pointInPolygonButton_Click(object sender, EventArgs e)
        {
            // 获得点图层
            int PointId = 1;
            IFeatureLayer PointLayer = null;
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                PointLayer = axMapControl1.get_Layer(i) as IFeatureLayer;
                if (PointLayer is IFeatureLayer && PointLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    PointId = i;
                }
            }
            if (PointId == -1)
            {
                MessageBox.Show("找不到质点图层，请重新加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 获得面图层
            int PolygonId = -1;
            IFeatureLayer PolygonLayer = null;
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                PolygonLayer = axMapControl1.get_Layer(i) as IFeatureLayer;
                if (PolygonLayer is IFeatureLayer && PolygonLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    PolygonId = i;
                }
            }
            if (PolygonId == -1)
            {
                MessageBox.Show("找不到边界图层，请重新加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PointLayer = axMapControl1.get_Layer(PointId) as IFeatureLayer;
            PolygonLayer = axMapControl1.get_Layer(PolygonId) as IFeatureLayer;

            IFeatureCursor PointFeatureCursor = PointLayer.Search(null, true);
            IFeature PointFeature = PointFeatureCursor.NextFeature();

            IFeatureCursor PolygonFeatureCursor = PolygonLayer.Search(null, true);
            IFeature PolygonFeature = PolygonFeatureCursor.NextFeature();

            IRelationalOperator pRelationalOperator = PolygonFeature.Shape as IRelationalOperator;

            if (pRelationalOperator.Contains(PointFeature.Shape))
            {
                MessageBox.Show("点在面内！", "恭喜你！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show("点在面外！", "很遗憾！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
