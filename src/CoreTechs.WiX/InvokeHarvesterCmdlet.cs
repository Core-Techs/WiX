using System.Management.Automation;

namespace CoreTechs.WiX
{
    [Cmdlet(VerbsLifecycle.Invoke, "Harvester")]
    public class InvokeHarvesterCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string DirectoryPath { get; set; }

        [Parameter(Mandatory = true)]
        public string WxsDestinationFilePath { get; set; }

        [Parameter(Mandatory = true)]
        public string DirectoryRefId { get; set; }

        [Parameter(Mandatory = true)]
        public string ComponentGroupId { get; set; }

        [Parameter]
        public string[] IncludeFiles { get; set; }

        [Parameter]
        public string[] ExcludeFiles { get; set; }

        [Parameter]
        public string XSLTFilePath { get; set; }

        [Parameter]
        public ITransformation Transformation { get; set; }

        protected override void ProcessRecord()
        {
            var h = new Harvester(DirectoryPath, WxsDestinationFilePath, DirectoryRefId, ComponentGroupId)
            {
                IncludeFiles = IncludeFiles,
                ExcludeFiles = ExcludeFiles,
                XSLTFilePath = XSLTFilePath,
                Transformation = Transformation
            };

            var xml = h.Harvest();

            WriteObject(xml);
        }
    }
}