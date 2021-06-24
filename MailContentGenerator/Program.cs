using System;
using System.Collections.Generic;
using System.IO;

namespace MailContentGenerator
{
    class DataPath
    {
        private static string _instance;
        private DataPath() { }

        public static string Instance()
        {
            if(_instance == null)
            {
                string directory = @"data";
                _instance = Path.GetFullPath(directory);
            }
            return _instance;
        }
    }
    class Customer
    {
        private string _name;
        private string _gender;
        private int _age;
        private string _nationality;

        public string Name{
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Gender {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
            }
        }
        public int Age {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
        public string Nationality {
            get
            {
                return _nationality;
            }
            set
            {
                _nationality = value;
            }
        }

        public Customer () { }

        public Customer(string name, string gender, int age, string nationality)
        {
            Name = name;
            Gender = gender;
            Age = age;
            Nationality = nationality;
        }
    }
    class CSVReader
    {
        private string _fileName;
        public CSVReader(string fileName)
        {
            _fileName = fileName;
        }
        public List<Customer> ReadCSV()
        {
            string dataPath = DataPath.Instance();

            List<Customer> result = new List<Customer>();
            using (var reader = new StreamReader(dataPath + "\\" + _fileName))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    int age = Convert.ToInt32(values[2]);
                    Customer newCustomer = new Customer(values[0], values[1], age, values[3]);
                    result.Add(newCustomer);
                }
            }
            return result;
        }
    }
    abstract class Mail
    {
        protected string _head;
        protected string _body;
        protected string _end;

        public string Head { get => _head; set => _head = value; }
        public string Body { get => _body; set => _body = value; }
        public string End { get => _end; set => _end= value; }

        protected Mail() { }

        public void PrintMail()
        {
            Console.WriteLine(_head);
            Console.WriteLine(_body);
            Console.WriteLine(_end);
        }
    }

    class ForeignMail: Mail
    {

    }
    class VNMaleMail: Mail
    {

    }
    class VNFemaleU30Mail: Mail
    {

    }
    class VNFemailMail: Mail
    {

    }

    abstract class MailFactory
    {
        protected Customer _customer;
        public abstract Mail CreateMail();
    }

    class ForeignMailFactory : MailFactory
    {
        public ForeignMailFactory(Customer cus)
        {
            _customer = cus;
        }
        public override Mail CreateMail()
        {
            MailTemplate template = ForeignTemplate.GetInstance();
            MailDecorStrategy strategy;
            Mail newMail = new ForeignMail();

            if(_customer.Gender == "Nam")
            {
                if (_customer.Age < 30) strategy = new MaleU30();
                else strategy = new Male();
            }
            else strategy = new Female();

            strategy.Name = _customer.Name;
            newMail.Head = strategy.DecorHead(template.Head);
            newMail.Body = strategy.DecorBody(template.Body);
            newMail.End = strategy.DecorEnd(template.End);
            return newMail;
        }
    }

    class VNMaleMailFactory : MailFactory
    {
        public VNMaleMailFactory(Customer cus)
        {
            _customer = cus;
        }
        public override Mail CreateMail()
        {
            MailTemplate template = VNMaleTemplate.GetInstance();
            MailDecorStrategy strategy;
            Mail newMail = new VNMaleMail();

            if (_customer.Gender == "Nam")
            {
                if (_customer.Age < 30) strategy = new MaleU30();
                else strategy = new Male();
            }
            else strategy = new Female();

            strategy.Name = _customer.Name;
            newMail.Head = strategy.DecorHead(template.Head);
            newMail.Body = strategy.DecorBody(template.Body);
            newMail.End = strategy.DecorEnd(template.End);
            return newMail;
        }
    }

    class VNFemaleMailFactory : MailFactory
    {
        public VNFemaleMailFactory(Customer cus)
        {
            _customer = cus;
        }
        public override Mail CreateMail()
        {
            MailTemplate template = VNFemaleTemplate.GetInstance();
            MailDecorStrategy strategy;
            Mail newMail = new VNFemailMail();

            if (_customer.Gender == "Nam")
            {
                if (_customer.Age < 30) strategy = new MaleU30();
                else strategy = new Male();
            }
            else strategy = new Female();

            strategy.Name = _customer.Name;
            newMail.Head = strategy.DecorHead(template.Head);
            newMail.Body = strategy.DecorBody(template.Body);
            newMail.End = strategy.DecorEnd(template.End);
            return newMail;
        }
    }
    class VNFemaleU30MailFactory : MailFactory
    {
        public VNFemaleU30MailFactory(Customer cus)
        {
            _customer = cus;
        }
        public override Mail CreateMail()
        {
            MailTemplate template = VNFemaleU30Template.GetInstance();
            MailDecorStrategy strategy;
            Mail newMail = new VNFemaleU30Mail();

            if (_customer.Gender == "Nam")
            {
                if (_customer.Age < 30) strategy = new MaleU30();
                else strategy = new Male();
            }
            else strategy = new Female();

            strategy.Name = _customer.Name;
            newMail.Head = strategy.DecorHead(template.Head);
            newMail.Body = strategy.DecorBody(template.Body);
            newMail.End = strategy.DecorEnd(template.End);
            return newMail;
        }
    }

    abstract class MailDecorStrategy
    {
        string _name;

        public string Name { get => _name; set => _name = value; }

        public abstract string DecorHead(string head);
        public abstract string DecorBody(string body);
        public abstract string DecorEnd(string end);
    }

    class Male: MailDecorStrategy
    {
        public override string DecorBody(string body)
        {
            string name = Name;
            string newBody = body.Replace("{name}", Name).Replace("{gender}", "Ong");
            return newBody;
        }

        public override string DecorEnd(string end)
        {
            string name = Name;
            string newBody = end.Replace("{name}", Name).Replace("{gender}", "Ong");
            return newBody;
        }

        public override string DecorHead(string head)
        {
            string name = Name;
            string newBody = head.Replace("{name}", Name).Replace("{gender}", "Ong");
            return newBody;
        }
    }
    class MaleU30 : MailDecorStrategy
    {
        public override string DecorBody(string body)
        {
            string name = Name;
            string newBody = body.Replace("{name}", Name).Replace("{gender}", "Anh");
            return newBody;
        }

        public override string DecorEnd(string end)
        {
            string name = Name;
            string newBody = end.Replace("{name}", Name).Replace("{gender}", "Anh");
            return newBody;
        }

        public override string DecorHead(string head)
        {
            string name = Name;
            string newBody = head.Replace("{name}", Name).Replace("{gender}", "Anh");
            return newBody;
        }
    }
    class Female : MailDecorStrategy
    {
        public override string DecorBody(string body)
        {
            string name = Name;
            string newBody = body.Replace("{name}", Name).Replace("{gender}", "Chi");
            return newBody;
        }

        public override string DecorEnd(string end)
        {
            string name = Name;
            string newBody = end.Replace("{name}", Name).Replace("{gender}", "Chi");
            return newBody;
        }

        public override string DecorHead(string head)
        {
            string name = Name;
            string newBody = head.Replace("{name}", Name).Replace("{gender}", "Chi");
            return newBody;
        }
    }
    abstract class MailTemplate
    {
        private string _head;
        private string _body;
        private string _end;

        public string Head { get => _head; set => _head = value; }
        public string Body { get => _body; set => _body = value; }
        public string End { get => _end; set => _end = value; }

        protected MailTemplate() { }

    }
    class ForeignTemplate : MailTemplate
    {
        static ForeignTemplate _instance;
        private ForeignTemplate()
        {
            string dataPath = DataPath.Instance();
            using (var headReader = new StreamReader(dataPath + "\\" + "Head_EN.txt"))
            {
                while (!headReader.EndOfStream)
                {
                    Head += headReader.ReadLine();
                    Head += Environment.NewLine;
                }
            }
            using (var bodyReader = new StreamReader(dataPath + "\\" + "Body_EN.txt"))
            {
                while (!bodyReader.EndOfStream)
                {
                    Body += bodyReader.ReadLine();
                    Body += Environment.NewLine;
                }
            }
            using (var endReader = new StreamReader(dataPath + "\\" + "Ending_EN.txt"))
            {
                while (!endReader.EndOfStream)
                {
                    End += endReader.ReadLine();
                    End += Environment.NewLine;
                }
            }

        }
        public static ForeignTemplate GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ForeignTemplate() ;
            }

            return _instance;
        }
    }

    class VNMaleTemplate : MailTemplate
    {
        static VNMaleTemplate _instance;
        private VNMaleTemplate()
        {
            string dataPath = DataPath.Instance();
            using (var headReader = new StreamReader(dataPath + "\\" + "Head_VN.txt"))
            {
                while (!headReader.EndOfStream)
                {
                    Head += headReader.ReadLine();
                    Head += Environment.NewLine;
                }
            }
            using (var bodyReader = new StreamReader(dataPath + "\\" + "Body_VN.txt"))
            {
                while (!bodyReader.EndOfStream)
                {
                    Body += bodyReader.ReadLine();
                    Body += Environment.NewLine;
                }
            }
            using (var endReader = new StreamReader(dataPath + "\\" + "Ending_VN.txt"))
            {
                while (!endReader.EndOfStream)
                {
                    End += endReader.ReadLine();
                    End += Environment.NewLine;
                }
            }
        }
        public static VNMaleTemplate GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VNMaleTemplate();
            }

            return _instance;
        }
    }

    class VNFemaleU30Template: MailTemplate
    {
        static VNFemaleU30Template _instance;
        private VNFemaleU30Template()
        {
            string dataPath = DataPath.Instance();
            using (var headReader = new StreamReader(dataPath + "\\" + "Head_VN.txt"))
            {
                while (!headReader.EndOfStream)
                {
                    Head += headReader.ReadLine();
                    Head += Environment.NewLine;
                }
            }
            using (var bodyReader = new StreamReader(dataPath + "\\" + "BodyU30_VN.txt"))
            {
                while (!bodyReader.EndOfStream)
                {
                    Body += bodyReader.ReadLine();
                    Body += Environment.NewLine;
                }
            }
            using (var endReader = new StreamReader(dataPath + "\\" + "Ending_VN.txt"))
            {
                while (!endReader.EndOfStream)
                {
                    End += endReader.ReadLine();
                    End += Environment.NewLine;
                }
            }
        }
        public static VNFemaleU30Template GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VNFemaleU30Template();
            }

            return _instance;
        }
    }

    class VNFemaleTemplate: MailTemplate
    {
        static VNFemaleTemplate _instance;
        private VNFemaleTemplate()
        {
            string dataPath = DataPath.Instance();
            using (var headReader = new StreamReader(dataPath + "\\" + "Head_VN.txt"))
            {
                while (!headReader.EndOfStream)
                {
                    Head += headReader.ReadLine();
                    Head += Environment.NewLine;
                }
            }
            using (var bodyReader = new StreamReader(dataPath + "\\" + "Body30_VN.txt"))
            {
                while (!bodyReader.EndOfStream)
                {
                    Body += bodyReader.ReadLine();
                    Body += Environment.NewLine;
                }
            }
            using (var endReader = new StreamReader(dataPath + "\\" + "Ending_VN.txt"))
            {
                while (!endReader.EndOfStream)
                {
                    End += endReader.ReadLine();
                    End += Environment.NewLine;
                }
            }
        }
        public static VNFemaleTemplate GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VNFemaleTemplate();
            }

            return _instance;
        }
    }

    class MassMailGenerator
    {
        string _csvFile;
        List<Customer> _listCustomer;

        public MassMailGenerator(string csvFile)
        {
            _csvFile = csvFile;
            _listCustomer = GetCustomerList();
        }

        public List<Customer> GetCustomerList()
        {
            CSVReader input = new CSVReader(_csvFile);
            return input.ReadCSV();
        }

        public Dictionary<string, Mail> GetAllMail()
        {
            MailFactory factory;
            Dictionary<string, Mail> result = new Dictionary<string, Mail>();
            foreach(Customer cus in _listCustomer)
            {
                if (cus.Nationality == "VN")
                {
                    if (cus.Gender == "Nam") factory = new VNMaleMailFactory(cus);
                    else
                    {
                        if (cus.Age < 30) factory = new VNFemaleU30MailFactory(cus);
                        else factory = new VNFemaleMailFactory(cus);
                    }
                }
                else factory = new ForeignMailFactory(cus);
                Mail newMail = factory.CreateMail();
                result.Add(cus.Name, newMail);
            }
            return result;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MassMailGenerator engine = new MassMailGenerator("khachhang.csv");
            Dictionary<string, Mail> mailList = engine.GetAllMail();
            foreach(KeyValuePair<string,Mail> mail in mailList)
            {
                Console.WriteLine(mail.Key + ":");
                mail.Value.PrintMail();
            }
        }
    }
}
