HSBuddy v0.1
=================

Keep track of played cards in Hearthstone.

---
Current Status:
---
VERSION: Extreme Alpha
UPDATED: 5/28/14 15:40 EST

I'm going to create the default option of reading from the Hearthstone log files, and only include the TCP option as a fallback. This would make HSBuddy the only program that does not violate the Blizzard TOS. Details on how I'm going to do that are here:

http://www.reddit.com/r/hearthstone/comments/268fkk/simple_hearthstone_logging_see_your_complete_play/

---
Reminders
---

Before explaining how the project works, I'd like to remind everyone of a few things:

- I'm bad at C#
- Most of this will be completely ripped out
- It's primarily just to demonstrate how to parse game data in its current form

So please feel free to contribute/fork.

---
Where I'd like it to go
---

I see the end product being the following:

- Two windows docked around the Hearthstone interface - one on the left for your opponent's (predicted) decklist, one on the right for your (known) list.
- Easy importing of decklists (screenshots are ideal, but at least give the option for a file)
- For opponent's possible decks, include common variation of cards. This would contain a "decision tree" where if Card A or Card B is played, then the other is likely included, but not Card C.
- Use the information I've included in hd.csv to calculate the maximum theoretical damage one can expect next turn, and display it in the window.
- *A much, much sexier interface*

---
How it Works
---

The steps are pretty simple for the most part, with the exception of the actual Protobuf decoding:

- Load up some of Hearthstone's assemblies so we can use their premade classes
- Create a list of user-defined decks
- Load up the deck you're playing
- Monitor TCP traffic on ports 1119 and 3724 (Blizzard's ports)
- Wait for a Pegasus game message (identified by 3 fields: a message length, a message type, and the message itself)
- Decode that message and see if it's one of the types we know about (all those in the "setuplistener" class. For instance, Type 19 is the PowerHistory message type, and contains what we want about game plays).
- Use the Hearthstone classes we got from the DLL's to easily break the message into key:value pairs

Specifically, I use a packetdecoder dictionary with the ConnectAPI. An example decoder looks like this:
    
    s_packetDecoders.Add(1, new ConnectAPI.DefaultProtobufPacketDecoder<GetGameState, GetGameState.Builder>());

An example message after being decoded might look like this:

    Type: 19
    Body: show_entity { entity: 63 name: "EX1_007" tags { name: 32 value: 1 } tags { name: 45 value: 3 } tags { name: 47 value: 1 } tags { name: 48 value: 3 } tags { name: 49 value: 3 } tags { name: 202 value: 4 } tags { name: 203 value: 1 } }
 
Let's break that down a little bit.

First, whenever a card is drawn (you) or played (opponent), the game registers it into an "entity" field. We need to track that to determine when it is played later. Here, it is assigned a value of 63.

Next, it tells us which card it is. EX1_007 isn't very useful by itself, but the game data contains all this information within its unity3d files (these can be decompiled with Disunity and inspected). I've included a full list in hd.csv. In this example, EX1_007 is Acolyte of Pain.

The rest are just values assigned to the card. These are defined within the Hearthstone DLL's, which can be decompiled with a program like .Net Reflector. The .cs file in particular here is GAMETAGS.cs. Here's an example of what that looks like:

    public enum GAME_TAG
    {
        ...
        HEALING_DOUBLE = 357,
        HEALTH = 45,
        HEALTH_MINIMUM = 337,
        HERO_ENTITY = 27,
        ...
    }

So the tag "45" above with a value of "3" is Acolyte's health, which we know is correct.

The rest of the tags don't really matter for our purposes right now, but can be used later.

The other type of message I like to track is "PowerStart", which basically means you've played a card. In this program at the moment, I mark the cards off as they are played rather than drawn, which can easily be changed:

    power_start { type: PLAY index: 0 source: 63 target: 0 }
    
Which just means that entity_id 63 (Acolyte) was played.


And here is a video of the proof of concept in action:

https://www.youtube.com/watch?v=q8HpCuFPlQo



