using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Lists the countries of the game. Countries are by default enemies.
/// </summary>
[InitializeOnLoad]
[ExecuteAlways]
public class Faction : ScriptableObject
{
   #region Enumerations

   /// <summary>
   /// Countries in the game.
   /// </summary>
   public enum CountryName { Gomer, Scythia, Madesh, Javan, Tobolik, Meshchera, Thuras, Cush, Mizr, Huttia, Palesti, Elam, Asshuria, Arphaxia, Lydda, Aram }

   /// <summary>
   /// Alliances countries can be part of.
   /// </summary>
   public enum Coalition { Neutral, None, Allied, United, International, Cosmopolitan, Federal, Joint }

   /// <summary>
   /// What a target is in relation to another target.
   /// </summary>
   public enum Identification { Neutral, Friend, Foe }

   #endregion


   #region Member Variables

   public static Dictionary<CountryName, Faction> m_CountryList = new Dictionary<CountryName, Faction>() {
      { CountryName.Gomer, new Faction(CountryName.Gomer, s_RoundelGomer) },
      { CountryName.Scythia, new Faction(CountryName.Scythia, s_RoundelNone) },
      { CountryName.Madesh, new Faction(CountryName.Madesh, s_RoundelNone) },
      { CountryName.Javan, new Faction(CountryName.Javan, s_RoundelNone) },
      { CountryName.Tobolik, new Faction(CountryName.Tobolik, s_RoundelNone) },
      { CountryName.Meshchera, new Faction(CountryName.Meshchera, s_RoundelNone) },
      { CountryName.Thuras, new Faction(CountryName.Thuras, s_RoundelNone) },
      { CountryName.Cush, new Faction(CountryName.Cush, s_RoundelNone) },
      { CountryName.Mizr, new Faction(CountryName.Mizr, s_RoundelNone) },
      { CountryName.Huttia, new Faction(CountryName.Huttia, s_RoundelNone) },
      { CountryName.Palesti, new Faction(CountryName.Palesti, s_RoundelNone) },
      { CountryName.Elam, new Faction(CountryName.Elam, s_RoundelNone) },
      { CountryName.Asshuria, new Faction(CountryName.Asshuria, s_RoundelNone) },
      { CountryName.Arphaxia, new Faction(CountryName.Arphaxia, s_RoundelNone) },
      { CountryName.Lydda, new Faction(CountryName.Lydda, s_RoundelNone) },
      { CountryName.Aram, new Faction(CountryName.Aram, s_RoundelNone) }
   };

   [SerializeField] Material m_RoundelGomer;
   [SerializeField] Material m_RoundelNone;

   private static Material s_RoundelGomer;
   private static Material s_RoundelNone;

   #endregion


   #region Properties

   public string Demonym { get; private set; }
   public string Capital { get; private set; }
   public CountryName Name { get; private set; }
   public Coalition CountryCoalition { get; private set; }
   public Material Roundel { get; private set; }

   #endregion


   #region Constructor

   private void OnEnable()
   {
      s_RoundelGomer = m_RoundelGomer;
      s_RoundelNone = m_RoundelNone;
   }

   /// <summary>
   /// Create a country with a set name, demonym, capital, and coalition.
   /// </summary>
   /// <param name="country">The country to be created</param>
   public Faction(CountryName country, Material roundel)
   {
      Debug.Log("New faction");
      switch(country)
      {
         case CountryName.Gomer:
            Name = CountryName.Gomer;
            Demonym = "Gomerag";
            Capital = "Galic";
            CountryCoalition = Coalition.Allied;
            Roundel = roundel;
            break;

         case CountryName.Scythia:
            Name = CountryName.Scythia;
            Demonym = "Scyth";
            Capital = "Ukra";
            CountryCoalition = Coalition.Allied;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Madesh:
            Name = CountryName.Madesh;
            Demonym = "Madai";
            Capital = "Madapa";
            CountryCoalition = Coalition.None;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Javan:
            Name = CountryName.Javan;
            Demonym = "Javanite";
            Capital = "Elys";
            CountryCoalition = Coalition.United;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Tobolik:
            Name = CountryName.Tobolik;
            Demonym = "Tabali";
            Capital = "Tobolsk";
            CountryCoalition = Coalition.United;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Meshchera:
            Name = CountryName.Meshchera;
            Demonym = "Meshchek";
            Capital = "Meshkva";
            CountryCoalition = Coalition.International;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Thuras:
            Name = CountryName.Thuras;
            Demonym = "Thuran";
            Capital = "Thur";
            CountryCoalition = Coalition.International;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Cush:
            Name = CountryName.Cush;
            Demonym = "Cushite";
            Capital = "Ethi";
            CountryCoalition = Coalition.International;
            Roundel = s_RoundelNone;
            break;

         case CountryName.Mizr:
            Name = CountryName.Mizr;
            Demonym = "Mizraim";
            Capital = "Mitz";
            CountryCoalition = Coalition.Cosmopolitan;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Huttia:
            Name = CountryName.Huttia;
            Demonym = "Huttite";
            Capital = "Hut";
            CountryCoalition = Coalition.Cosmopolitan;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Palesti:
            Name = CountryName.Palesti;
            Demonym = "Palesim";
            Capital = "Jebussa";
            CountryCoalition = Coalition.None;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Elam:
            Name = CountryName.Elam;
            Demonym = "Elam";
            Capital = "Eru";
            CountryCoalition = Coalition.Federal;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Asshuria:
            Name = CountryName.Asshuria;
            Demonym = "Asshur";
            Capital = "Ashkh";
            CountryCoalition = Coalition.Federal;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Arphaxia:
            Name = CountryName.Arphaxia;
            Demonym = "Arip";
            Capital = "Chalda";
            CountryCoalition = Coalition.Federal;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Lydda:
            Name = CountryName.Lydda;
            Demonym = "Sardi";
            Capital = "Lydd";
            CountryCoalition = Coalition.Joint;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Aram:
            Name = CountryName.Aram;
            Demonym = "Arami";
            Capital = "Aramecc";
            CountryCoalition = Coalition.Joint;
            Roundel = m_RoundelNone;
            break;
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Compare two countries to find whether they are neutral, friend, or foe.
   /// </summary>
   /// <param name="known">The reference country</param>
   /// <param name="country">The country being compared</param>
   /// <returns></returns>
   public Identification Relationship(Faction country)
   {
      // When any or both sides have no coalition, they are enemies
      if(CountryCoalition == Coalition.None || country.CountryCoalition == Coalition.None)
         return Identification.Foe;

      // When any or both sides are neutral, both sides are neutral to each other
      if(CountryCoalition == Coalition.Neutral || country.CountryCoalition == Coalition.Neutral)
         return Identification.Neutral;

      // When coalitions don't match, they are enemies
      else if(CountryCoalition != country.CountryCoalition)
         return Identification.Foe;

      // Coalitions match
      else
         return Identification.Friend;
   }

   #endregion
}