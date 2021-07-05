namespace WinFormsAppSinuca
{
	public class Competidor : ICompetidor
	{
		//declarations
		public ITime Time { get; set; }
		public int Pontos { get; set; }

		//constructor
		public Competidor()
		{
			this.Time = new Time();
			this.Pontos = 0;
		}

		//methods
		public override string ToString()
		{
			return Time.ToString();
		}
	}
}
