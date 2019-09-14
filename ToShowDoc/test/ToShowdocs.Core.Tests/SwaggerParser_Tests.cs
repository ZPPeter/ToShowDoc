using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using ToShowDoc.Core.Parser;
using Xunit;

namespace ToShowDocs.Core.Tests
{
    public class SwaggerParser_Tests
    {
        private readonly SwaggerDocument _document;
        public SwaggerParser_Tests()
        {
            var str = File.ReadAllText(AppContext.BaseDirectory + "swaggerDocs.json");
            _document = SwaggerParser.ParseString(str);
        }

        [Fact]
        public void SwaggerDocsDefinition_Test()
        {
            _document.Definitions.First().Key.ShouldBe("IsTenantAvailableInput");

            _document.Definitions.First().Value.Required.ShouldBe(new[] { "tenancyName" });
            _document.Definitions.First().Value.Type.ShouldBe("object");
            _document.Definitions.First().Value.Properties.First().Key.ShouldBe("tenancyName");
            _document.Definitions.First().Value.Properties.First().Value.MaxLength.ShouldBe(64);
            _document.Definitions.First().Value.Properties.First().Value.MinLength.ShouldBe(0);
            _document.Definitions.First().Value.Properties.First().Value.Type.ShouldBe("string");

            _document.Definitions["IsTenantAvailableOutput"].Properties["state"].Enum.Length.ShouldBe(3);
            _document.Definitions["IsTenantAvailableOutput"].Properties["state"].Type.ShouldBe("integer");
        }

        [Fact]
        public void SwaggerDocsPath_Test()
        {
            _document.Paths["/api/TokenAuth/Authenticate"].Get.ShouldBeNull();
            _document.Paths["/api/TokenAuth/Authenticate"].Delete.ShouldBeNull();
            _document.Paths["/api/TokenAuth/Authenticate"].Put.ShouldBeNull();

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Consumes.Length.ShouldBe(4);
            _document.Paths["/api/TokenAuth/Authenticate"].Post.OperationId.ShouldBe("Authenticate");

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.Count.ShouldBe(1);
            ((string)_document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().@in).ShouldBe("body");
            ((string)_document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().name).ShouldBe("model");
            ((bool)_document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().required).ShouldBe(false);
            ((string)_document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().schema["$ref"]).ShouldBe("#/definitions/AuthenticateModel");

            string des = _document.Paths["/api/TokenAuth/Authenticate"].Post.Responses["200"].description.ToString();
            string @ref = _document.Paths["/api/TokenAuth/Authenticate"].Post.Responses["200"].schema["$ref"].ToString();
            des.ShouldBe("Success");
            @ref.ShouldBe("#/definitions/AuthenticateResultModel");
        }
    }
}
