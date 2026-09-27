using System;

namespace CSharp20Tutorial.OOP
{
    /// <summary>
    /// 封装示例
    /// 通过访问修饰符隐藏内部实现细节，只暴露必要的接口
    /// </summary>
    
    // 银行账户类：封装账户数据和操作
    class BankAccount
    {
        // 私有字段：外部不能直接访问
        private string _accountNumber;
        private string _ownerName;
        private decimal _balance;
        private string _password;
        
        // 构造函数
        public BankAccount(string accountNumber, string ownerName, string password, decimal initialDeposit)
        {
            _accountNumber = accountNumber;
            _ownerName = ownerName;
            _password = password;
            _balance = initialDeposit;
        }
        
        // 公共只读属性：只能读取不能修改余额
        public decimal Balance
        {
            get { return _balance; }
        }
        
        public string OwnerName
        {
            get { return _ownerName; }
        }
        
        public string AccountNumber
        {
            get { return _accountNumber; }
        }
        
        // 公共方法：存款
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("存款金额必须大于0！");
                return;
            }
            _balance += amount;
            Console.WriteLine("存款成功！存入{0:C}，当前余额：{1:C}", amount, _balance);
        }
        
        // 公共方法：取款（需要密码验证）
        public bool Withdraw(decimal amount, string password)
        {
            if (password != _password)
            {
                Console.WriteLine("密码错误！取款失败。");
                return false;
            }
            
            if (amount <= 0)
            {
                Console.WriteLine("取款金额必须大于0！");
                return false;
            }
            
            if (amount > _balance)
            {
                Console.WriteLine("余额不足！当前余额{0:C}", _balance);
                return false;
            }
            
            _balance -= amount;
            Console.WriteLine("取款成功！取出{0:C}，当前余额：{1:C}", amount, _balance);
            return true;
        }
        
        // 修改密码
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword != _password)
            {
                Console.WriteLine("原密码错误，修改失败！");
                return false;
            }
            
            _password = newPassword;
            Console.WriteLine("密码修改成功！");
            return true;
        }
        
        // 查询余额需要密码
        public void QueryBalance(string password)
        {
            if (password != _password)
            {
                Console.WriteLine("密码错误！");
                return;
            }
            Console.WriteLine("账户{0}，当前余额：{1:C}", _accountNumber, _balance);
        }
    }
    
    class EncapsulationDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 封装示例：银行账户 =====");
            // 创建账户
            BankAccount myAccount = new BankAccount("6222021234567890", "张三", "123456", 1000);
            Console.WriteLine("账户创建成功！户主：{0}，账号：{1}", myAccount.OwnerName, myAccount.AccountNumber);
            Console.WriteLine("初始余额：{0:C}", myAccount.Balance);
            
            Console.WriteLine("\n--- 存款操作 ---");
            myAccount.Deposit(500);
            myAccount.Deposit(-100);  // 非法金额
            
            Console.WriteLine("\n--- 取款操作 ---");
            myAccount.Withdraw(200, "123456");
            myAccount.Withdraw(200, "111111");  // 密码错误
            myAccount.Withdraw(2000, "123456");  // 余额不足
            
            Console.WriteLine("\n--- 查询余额 ---");
            myAccount.QueryBalance("123456");
            
            Console.WriteLine("\n--- 修改密码 ---");
            myAccount.ChangePassword("123456", "654321");
            myAccount.Withdraw(100, "654321");
            
            // 外部无法直接访问私有字段，保证数据安全
            // myAccount._balance = 1000000;  // 编译错误！无法访问私有成员
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
