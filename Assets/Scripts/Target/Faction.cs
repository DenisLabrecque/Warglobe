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
      { CountryName.Gomer, new Faction(CountryName.Gomer) },
      { CountryName.Scythia, new Faction(CountryName.Scythia) },
      { CountryName.Madesh, new Faction(CountryName.Madesh) },
      { CountryName.Javan, new Faction(CountryName.Javan) },
      { CountryName.Tobolik, new Faction(CountryName.Tobolik) },
      { CountryName.Meshchera, new Faction(CountryName.Meshchera) },
      { CountryName.Thuras, new Faction(CountryName.Thuras) },
      { CountryName.Cush, new Faction(CountryName.Cush) },
      { CountryName.Mizr, new Faction(CountryName.Mizr) },
      { CountryName.Huttia, new Faction(CountryName.Huttia) },
      { CountryName.Palesti, new Faction(CountryName.Palesti) },
      { CountryName.Elam, new Faction(CountryName.Elam) },
      { CountryName.Asshuria, new Faction(CountryName.Asshuria) },
      { CountryName.Arphaxia, new Faction(CountryName.Arphaxia) },
      { CountryName.Lydda, new Faction(CountryName.Lydda) },
      { CountryName.Aram, new Faction(CountryName.Aram) }
   };

   [SerializeField] Material m_RoundelNone;
   [SerializeField] Material m_RoundelGomer;
   [SerializeField] Material m_RoundelScythia;
   [SerializeField] Material m_RoundelMadesh;
   [SerializeField] Material m_RoundelJavan;
   [SerializeField] Material m_RoundelTobolik;
   [SerializeField] Material m_RoundelPalesti;

   public static Material RoundelNone { get; private set; }

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
      RoundelNone = m_RoundelNone;
   }

   /// <summary>
   /// Create a country with a set name, demonym, capital, and coalition.
   /// </summary>
   /// <param name="country">The country to be created</param>
   public Faction(CountryName country)
   {
      switch(country)
      {
         case CountryName.Gomer:
            Name = CountryName.Gomer;
            Demonym = "Gomerag";
            Capital = "Galic";
            CountryCoalition = Coalition.Allied;
            Roundel = m_RoundelGomer;
            break;

         case CountryName.Scythia:
            Name = CountryName.Scythia;
            Demonym = "Scyth";
            Capital = "Ukra";
            CountryCoalition = Coalition.Allied;
            Roundel = m_RoundelScythia;
            break;

         case CountryName.Madesh:
            Name = CountryName.Madesh;
            Demonym = "Madai";
            Capital = "Madapa";
            CountryCoalition = Coalition.None;
            Roundel = m_RoundelMadesh;
            break;

         case CountryName.Javan:
            Name = CountryName.Javan;
            Demonym = "Javanite";
            Capital = "Elys";
            CountryCoalition = Coalition.United;
            Roundel = m_RoundelJavan;
            break;

         case CountryName.Tobolik:
            Name = CountryName.Tobolik;
            Demonym = "Tabali";
            Capital = "Tobolsk";
            CountryCoalition = Coalition.United;
            Roundel = m_RoundelTobolik;
            break;

         case CountryName.Meshchera:
            Name = CountryName.Meshchera;
            Demonym = "Meshchek";
            Capital = "Meshkva";
            CountryCoalition = Coalition.International;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Thuras:
            Name = CountryName.Thuras;
            Demonym = "Thuran";
            Capital = "Thur";
            CountryCoalition = Coalition.International;
            Roundel = m_RoundelNone;
            break;

         case CountryName.Cush:
            Name = CountryName.Cush;
            Demonym = "Cushite";
            Capital = "Ethi";
            CountryCoalition = Coalition.International;
            Roundel = m_RoundelNone;
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
            Demonym = "Philisim";
            Capital = "Jebussa";
            CountryCoalition = Coalition.None;
            Roundel = m_RoundelPalesti;
            break;

         case CountryName.Elam:
            Name = CountryName.Elam;
            Demonym = "Elami";
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
            Demonym = "Arrhip";
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