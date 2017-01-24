using System;
using System.Collections.Generic;
using System.Linq;
namespace ConsoleApplication
{
    // Create a Blackjack game that enables you to compete against the computer (the dealer). Here are the rules for Blackjack:
    // The goal of the game is to have a hand that has a value greater than the dealer but not greater than 21.  
    // The number cards have their face values. The Queen, King, Jack have the value 10. Finally, the Ace is worth either 1 or 11 (player's choice).
    // If a player's cards add up to over 21 then the player loses (busts).  
    // Each round, a player can either hit (take a new card) or stand (not take a card).  

// dealer turn:
// If the dealer doesn't have a natural, he hits (takes more cards) or stands depending on the value of the hand. Contrary to the player, though, the dealer's action is completely dictated by the rules. 
// The dealer must hit if the value of the hand is lower than 17, otherwise the dealer will stand.

    // flow: 
    //1. startgame(), 1. initialize new Dealer, 2. initialize new Player, promptUser()
    //2. create dealer hand 
    //3. create user hand
    //4. prompt user input
    //5. process user input
    //6. case nextRound()
    //7. case endGame()

//Each hand will result in one of the following events for the player:

// 1. Lose - 10  the player's bet is taken by the dealer.
//If the dealer has a natural 21 (blackjack) with his two cards, he won't take any more cards.
// All players lose, except players who also have a blackjack, in which case it is a push - the bet is returned to the player.

// Win + 10 2. - less than 21 but more than dealer. the player wins as much as he bet. If you bet $10, you win $10 from the dealer (plus you keep your original bet, of course.)

// Blackjack + 15 (natural) - gets 21. the player wins 1.5 times the bet. With a bet of $10, you keep your $10 and win a further $15 from the dealer.
// Push - the hand is a draw. The player keeps his bet, neither winning nor losing money.

    // tech:
    // 0. startgame(), 1. initialize new Dealer, 2. initialize new Player, 3. promptUser()
    // 1. Hand() class => 1. initializes with starting number of cards, 2. has a card List that's just a list of strings to add or remove 
    // 2. Hand.calculatePoints() 0=> called in constructor and with Hand.AddCard() => 
    // ==>  a. foreach in Cards list: 1.parses to int, 2. if no valid parse, try royal names, 3. no error handling, AddCard should do that
    //  ==> b. if ace ...
    //3. Hand.AddCard() => 1. is input a string? if not throw input error and reprompt, 2. is input a number from 1 to 10 or 
    // Player Class to handle prompts => 1. Player.Hand
    // Dealer Class to autogenerate data  => 1. Dealer.Hand
    // promptUser() => handle user Hand and Total and compare to Dealer Hand and Total. will either call EndGame(), or print some stuff and call itself
    // => a0. show your hand and facecard (card at [0]) for dealer
    // => a. ask for Hit or Stand =>
    // => b. if input is Ace, ask for 1 or 11 and addCard() from number
    // => c. if player.total over 21 player
    // endGame() => accepts 'win' or 'lose' parameters and notifies user based on that. stretch: ask for replay  
    public class Program
    {
     public static List<string> Cards {get; set;}

     public static Game game {get; set; }
        public class Game {
            // end method goes here  
            public Dealer dealer;
            public Player player;
            public void reset(){
                Console.Write("Your score is: ", player.score);
                player.hand = new Hand();
                dealer.hand = new Hand();
                dealer.hand.AddCard("dealer");
                dealer.hand.AddCard("dealer");
                player.hand.AddCard("player");
                player.hand.AddCard("player");
            }
            public void Lose(){
                player.score -= 10;
                Console.Write("You lose!\n");
               reset();
            }
            public void Push(){
                Console.Write("It is a push/tie!\n");
               reset();
            }
             public void Win(){
                 player.score += 5;
                Console.Write("You win!\n");
                reset();
            }
            public void BlackJack(){
                player.score +=10;
                Console.Write("You got blackjack!\n");
                reset();
            }
            public void Bust() {
                player.score -= 5;
                Console.Write("You busted!\n");
                reset();
            }
            public void dealerTurn(){
                Console.WriteLine("Dealer goes.");
                var pTotal = dealer.hand.calculateTotal();
                var dTotal = dealer.hand.calculateTotal();
                if(dTotal == 21){
                    Lose();
                    return;
                }
                //hit
                if(dTotal < 17){
                    dealer.hand.AddCard("dealer");
                }
                //else stand
                userPrompt();
            }
            public void tryHand(){
             var pTotal = dealer.hand.calculateTotal();
              var dTotal = dealer.hand.calculateTotal();
              if(pTotal <= 21 && pTotal > dTotal){
                  Win();
                  return;
              } 
               if(pTotal == 21 && pTotal != 21){
                  BlackJack();
                  return;
              } 
              if(pTotal == dTotal){
                  Push();
                  return;
              }
              if(pTotal > 21 || dTotal == 21){
                  Lose();
                  return;
              }
            }

            public void userPrompt(){
                Console.WriteLine("The dealer shows you a face card: " + dealer.hand.cards[0]);
                player.writeHand();
                Console.WriteLine("Make your Move: enter the word hit, stand, or tryhand.");
                var response = Console.ReadLine();
                if(response == "hit"){
                    player.hand.AddCard("player");
                    dealerTurn(); 
                } else if (response == "stand"){
                    dealerTurn(); 
                } else if (response == "tryhand"){
                    tryHand();
                } else {
                    Console.Write("Invalid response.");
                    userPrompt();
                }
            }
            public void askAce(){
                //TODO: askAce
                Console.WriteLine("You got an ace! blah blah blah!");
            }

            public Game (){
                player = new Player();
                dealer = new Dealer();
                userPrompt();
            }
        }

        public class Hand{
            public List<string> cards {get; set;}
            private int total {get; set;}
            public int calculateTotal () {
                foreach(string card in cards){
                    total = 0;
                    var parsed = 0;
                    var isParsed = int.TryParse(card, out parsed);
                    if(isParsed != false){
                        total += parsed;
                    } else if(card == "king" || card == "queen" || card == "jack"){
                        total += 10;
                    }
                }
                return total;
            }
            public void tryHandPrompt(){
                Console.WriteLine("Do you want to try your hand? y or n");
                var response = Console.ReadLine();
                if(response == "y"){
                    game.tryHand();
                } else if (response == "n"){
                    game.dealerTurn();
                } else {
                    Console.WriteLine("Sorry, did not understand.");
                    tryHandPrompt();
                }
            }
            public void AddCard(string userType){
                var random = new Random();
                var rnd =  random.Next(0, 12);
                var card = Cards[rnd];
                //bust handling
                if(userType == "player" && total > 21){
                    game.Bust();
                    return;
                }
                // ace handling
                if(card == "ace"){
                    if(userType == "player"){
                        game.askAce();
                    } else if (userType == "dealer"){
                        var _random = new Random();
                        var _rnd =  random.Next(0, 1);
                        if(_rnd == 0){
                            game.player.hand.AddCard("1");
                        } else {
                            game.player.hand.AddCard("11");
                        }
                          calculateTotal();
                    }
                } else {
                    if(userType == "player"){
                     game.player.hand.AddCard(Cards[rnd]);
                    } else if(userType == "dealer") {
                     game.dealer.hand.AddCard(Cards[rnd]);
                    }
                    calculateTotal();
                }
            }
            // public void RemoveCard(string card){
            //     var itemToRemove = cards.SingleOrDefault(c => c == card);
            //     if (itemToRemove != null){
            //       cards.Remove(card);
            //     }
            // }
            public Hand(){
                this.total = 0;
                List<string>cards = new List<string>();
            }
        }
         public class Player{
             public Hand hand;
              public int score {get; set;}
              public void writeHand() {
                 Console.WriteLine("Your hand: \n");
                 foreach(string card in hand.cards){
                     Console.WriteLine("\n" + card);
                 }
              }
             public Player(){
                 score = 0;
                 hand = new Hand();
                 hand.AddCard("player");
                 hand.AddCard("player");
             }
        }
        public class Dealer{
            public Hand hand;
            public int score {get; set;}
            public Dealer(){
                score = 0;
                hand = new Hand();
                hand.AddCard("dealer");
                hand.AddCard("dealer");
            }
        }
        public static void Main(string[] args)
        {   
            // initialize cards  
            var cards = new string [] {
                "2","3","4","5","6","7","8","9","king","queen","jack","ace"
           };
           List<string>Cards = new List<string>();
            foreach (string card in cards){
                Cards.Add(card);
            }
           //initialize game
           game = new Game();
    
            //Console.WriteLine("Hello World!");
        }
    }
}
