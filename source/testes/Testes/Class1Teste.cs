using Biblioteca;
using NUnit.Framework;

namespace Testes
{
    [TestFixture]
    public class Class1Teste
    {
        private Class1 class1;

        [SetUp]
        public void SetUp()
        {
            class1 = new Class1();
        }
        
        [Test]
        public void deve_testar_classe_1()
        {
            class1.Faz();
        }
    }
}