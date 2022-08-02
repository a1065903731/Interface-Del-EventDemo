using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceDemo
{
    /*
     * 接口:
    *对一组方法的声明，但没有具体的实现
    *如果一个类需要继承接口，那么必须实现类中所有的方法
    *
    *特点:
    * 1、接口中的成员不能使用访问修饰符
    * 2、接口成员可以是方法、属性、事件、索引器以及四种类型的混合，但不能够是字段、构造函数、析构函数、运算符重载
    * 3、不能够使用static关键字
    */
    interface ICustomCompare {
        int Compareto(object obj);
    }

    interface IChineseGreeting
    {
        void sayHello();
    }
    interface IAmericanGreeting {
        void sayHello();
    }
    public class Person : ICustomCompare
    {
        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public int Compareto(object obj)
        {
            if (obj == null) { return 1; }
            Person tempPerson = (Person)obj;
            if (this.Age > tempPerson.Age)
            {
                return 1;
            }
            else if (this.Age < tempPerson.Age)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class Greet : IChineseGreeting, IAmericanGreeting
    {
        void IChineseGreeting.sayHello()
        {
            Console.WriteLine("你好");
        }

        void IAmericanGreeting.sayHello()
        {
            Console.WriteLine("Hello");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //接口隐式调用
            //Test1();

            //接口显示调用
            //Test2();

            //委托例子
            //Test_Delegate();

            //事件例子
            Test_Event();
            Console.ReadKey();
        }

        public static void Test1()
        {
            Person p1 = new Person();
            p1.Age = 12;

            Person p2 = new Person();
            p2.Age = 16;

            if (p1.Compareto(p2) > 0)
            {
                Console.WriteLine("P1 greater than P2");
            }
            else if (p1.Compareto(p2) < 0)
            {
                Console.WriteLine("P1 less than P2");
            }
            else
            {
                Console.WriteLine("P1 equal to P2");
            }
        }

        public static void Test2()
        {
            Greet greet = new Greet();
            IChineseGreeting chinese = (IChineseGreeting)greet;
            chinese.sayHello();

            IAmericanGreeting american = (IAmericanGreeting)greet;
            american.sayHello();
        }

        //委托例子
        public static void Test_Delegate()
        {
            //以上实现方法的可扩展性很差，如果之后我们需要添加多个国家，那么维护起来比较复杂。有了委托，我们可以将函数作为参数，
            //并像下面的代码去实现Greeting方法了。
            MainClass1 p1 = new MainClass1();
            p1.Greeting("张三", "zh-cn");

            //使用委托例子
            MainClass2 p2 = new MainClass2();
            p2.Greeting("张三", p2.ChineseGreeting);
            p2.Greeting("lisi", p2.EnglishGreeting);
        }

        //事件例子
        public static void Test_Event()
        {
            BridgeGroom bridgeGroom = new BridgeGroom();
            Friend f1 = new Friend("张三");
            Friend f2 = new Friend("李四");
            Friend f3 = new Friend("王二");

            bridgeGroom.MarryEvent += new BridgeGroom.MarryHandler(f1.RecieveMsg);
            bridgeGroom.MarryEvent += new BridgeGroom.MarryHandler(f2.RecieveMsg);
            bridgeGroom.MarryEvent += new BridgeGroom.MarryHandler(f3.RecieveMsg);

            bridgeGroom.SendMarriageMsg("朋友们，我要结婚了");
        }
    }

    #region  委托例子相关类
    class MainClass1
    {
        public void Greeting(string name, string language)
        {
            switch (language)
            {
                case "zh-cn":
                    ChineseGreeting(name);
                    break;
                case "en-us" :
                    EnglishGreeting(name);
                    break;
                default:
                    break;
            }
        }

        public void EnglishGreeting(string name)
        {
            Console.WriteLine("hello"+name);
        }

        public void ChineseGreeting(string name)
        {
            Console.WriteLine("你好" + name);
        }
    }

    class MainClass2
    {
        public delegate void GreetingDelegate(string name);//定义委托类型
        public void Greeting(string name, GreetingDelegate del)
        {
            del(name);
        }
        public void EnglishGreeting(string name)
        {
            Console.WriteLine("hello" + name);
        }

        public void ChineseGreeting(string name)
        {
            Console.WriteLine("你好" + name);
        }
    }

    #endregion

    #region  事件例子相关类
    public class BridgeGroom
    {
        public delegate void MarryHandler(string msg);

        public event MarryHandler MarryEvent;

        public void SendMarriageMsg(string msg)
        {
            if (MarryEvent != null)
            {
                MarryEvent(msg);
            }
        }
    }

    public class Friend
    {
        public string name;
        public Friend(string _name)
        {
            name = _name;
        }
        public void RecieveMsg(string msg) {
            Console.WriteLine("msg:   " + msg);
            Console.WriteLine(this.name+"收到消息，到时准时参加");
        }
    }
    #endregion 
}
