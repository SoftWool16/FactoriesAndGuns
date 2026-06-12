using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factories_And_Guns
{
    enum TypeLearning
    {
        invisible, closed, locked, opened
    }
    class Object (Dictionary<string, Element> typesElementsForCrafting, BaseFactory factory, Object lastObject, TypeLearning typeLearning)
    {
        public Dictionary<string, Element> TypesElementsForCrafting { get; set; } = typesElementsForCrafting;
        public BaseFactory Factory { get; set; } = factory;
        public Object LastObject { get; set; } = lastObject;
        public TypeLearning TypeLearning { get; set; } = typeLearning;
    }
    internal class LearningSystem
    {
        static public string BackgroundPath { get; set; } = "User_Interface/background";
        static public Object[,] System = null;
    }
}
