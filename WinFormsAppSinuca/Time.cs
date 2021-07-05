namespace WinFormsAppSinuca
{
	public class Time : ITime
	{
		//declarations
		public string Nome { get; set; }
		public string Jogador1 { get; set; }
		public string Jogador2 { get; set; }

		//constructor
		public Time()
		{
			this.Nome = "";
			this.Jogador1 = "";
			this.Jogador2 = "";
		}

		//methods
		public override string ToString()
		{
			return this.Nome + " ( " + this.Jogador1 + " - " + this.Jogador2 + " ) ";
		}
	}
}
