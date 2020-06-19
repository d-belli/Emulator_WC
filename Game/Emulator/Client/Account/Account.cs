using System;

namespace Emulator {
	public class Account {
		// Atributos
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Numeric { get; set; }
		public Storage Storage { get; set; }
		public Character[] Characters { get; set; }

		// Construtor
		public Account ( ) {
			this.UserName = "";
			this.Password = "";
			this.Numeric = "";
			this.Storage = new Storage();
			this.Characters = new Character[4];
		}

		public Account(string userName, string password)
		{
			this.UserName = userName;
			this.Password = password;
			this.Numeric = "";
			this.Storage = new Storage();
			this.Characters = new Character[4];
		}
	}
}