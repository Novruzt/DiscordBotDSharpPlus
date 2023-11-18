using System;

namespace FIrstDiscordBotC_.Others
{
    public class CardSystem
    {
        private int[] CardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private string[] CardSuits = { "Clubs", "Spades", "Diamonds", "Hearts"};
        
        public int SelectedNumber { get; set; } 
        public string SelectedCard { get; set; }

        public CardSystem()
        {
              Random random = new Random();
              int numberIndex = random.Next(0, CardNumbers.Length-1);
              int suitIndex = random.Next(0, CardSuits.Length-1);
              
              this.SelectedNumber = CardNumbers[numberIndex];
              this.SelectedCard = $"{CardNumbers[numberIndex]} of {CardSuits[suitIndex]}";
        }
    }
}
