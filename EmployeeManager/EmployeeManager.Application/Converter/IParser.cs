namespace EmployeeManager.Application.Converter
{
	internal interface IParser<Origin, Destiny>
	{
		/// <summary>
		/// Parse an object from Origin to Destiny
		/// </summary>
		/// <param name="origin"></param>
		/// <returns></returns>
		Destiny Parse(Origin origin);

		/// <summary>
		/// Parse a collection of objects from Origin to Destiny
		/// </summary>
		/// <param name="origin"></param>
		/// <returns></returns>
		IEnumerable<Destiny> Parse(ICollection<Origin> origin);
	}
}
