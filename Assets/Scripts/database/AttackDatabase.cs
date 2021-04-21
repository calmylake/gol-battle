using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDatabase : MonoBehaviour
{
    List<Attack> Attacks;
    void Awake()
    {
        BuildElementDatabase();
    }

    public Attack GetAttack(int id)
    {
        return Attacks.Find(Attack => Attack.Id == id);
    }

    private void BuildElementDatabase()
    {
        Attacks = new List<Attack>() 
        { 
            /*
            category:
                damage:
                    1 => Magic
                    2 => Physical

                stats change:
                    3 => Strength
                    4 => Defense
                    5 => Magic
                    6 => Magic Defense
            */
            new Attack(1, "Vine Spear", 1, 80, 18, 3, 1, "A spear made out of the power of nature"),
            new Attack(2, "Dark Needles", 1, 60, 22, 11, 3, "Thin projectiles that can be very dangerous to your opponent"),
            new Attack(3, "Darkening Tornado", 1, 70, 25, 3, 2, "A powerful tornado from the shadows that cause massive damage"),
            new Attack(4, "Forbidden Contract", 3, 0, 30, 11, 1, "Increases the strength of a lifie by 50%", 0.5f),
            new Attack(5, "Cutting Light", 2, 90, 25, 10, 1, "A ray of light so thin that it cuts"),
            new Attack(6, "Bright Spikes", 2, 60, 22, 10, 3, "Spikes made of sunlight"),
            new Attack(7, "Teleport Fang", 1, 50, 30, 8, 4, "A fang capable of reaching great distances with the power of teleportation"),
            new Attack(8, "Blessed Support", 4, 0, 30, 10, 1, "Increases the defense of a lifie by 50%", 0.5f),
            new Attack(9, "Trident of The Seven", 2, 90, 25, 2, 1, "A trident using the power of the seven seas"),
            new Attack(10, "Mental Wave", 1, 70, 22, 7, 3, "A wave of power made from the concentration of the mind"),
            new Attack(11, "Water Arrow", 2, 60, 22, 2, 3, "A high pressure water jet that cuts like an arrow"),
            new Attack(12, "Flaming Ray", 1, 70, 22, 1, 2, "A hot ray that burns like fire"),
            new Attack(13, "Ancestral Fissure", 1, 70, 26, 4, 2, "A fissure made from the ancestral powers that build the Earth"),
            new Attack(14, "Supernova", 1, 120, 40, 1, 1, "A massive explosion"),
            new Attack(15, "Hydraulic Punch", 2, 100, 36, 5, 1, "A punch with the force of a hydraulic press"),
            new Attack(16, "Loving Missile", 2, 70, 26, 5, 2, "A special missile, fired with love"),
            new Attack(17, "Electric Pressure", 2, 90, 30, 6, 1, "An electrical blast coming from the claws"),
            new Attack(18, "Micro Bullets", 2, 70, 47, 5, 4, "A huge amount of bulets being fired rapidly"),
            new Attack(19, "Power Stomp", 2, 90, 31, 8, 1, "A powerful stomp that can cause massive damage"),
            new Attack(20, "Leaf Spike", 2, 70, 10, 3, 1, "An extremely rigid and pointed leaf"),
            new Attack(21, "Finishing Claw", 2, 140, 60, 8, 1, "A claw capable of piercing any surface")
        };
    }
}
