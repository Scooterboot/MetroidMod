namespace MetroidMod.Content
{
	public interface IGeneratesOnStatues
	{
		/// <summary>
		/// Determines the chance an addon can generate on Chozo Statues during world generation.
		/// </summary>
		/// <param name="x">The X location of the tile</param>
		/// <param name="y">The Y location of the tile</param>
		/// <param name="statueType">The variation of statue.</param>
		/// <param name="chozoRoom">Currently unused; may be used if random chozo statue rooms are implemented.</param>
		int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom);
		
		int ItemType { get; }
	}
}