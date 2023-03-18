using DocuSign.eSign.Client;

namespace Traki.Api.Handlers
{
    public interface IDocusignHandler
    {
    }

    public class DocusignHandler
    {
        public void CreateDocument()
        {
            var docuSignClient = new DocuSignClient("test");
        }
    }
}
