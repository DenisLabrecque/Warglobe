using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lists the countries of the game. Countries are by default enemies.
/// </summary>
[ExecuteAlways]
public class Country : ScriptableObject
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

   public static Dictionary<CountryName, Country> m_CountryList;

   [SerializeField] Material m_RoundelGomer;
   [SerializeField] Material m_RoundelScythia;
   [SerializeField] Material m_RoundelMadesh;
   [SerializeField] Material m_RoundelJavan;
   [SerializeField] Material m_RoundelTobolik;
   [SerializeField] Material m_RoundelMeshchera;
   
   #endregion


   #region Properties

   public string Demonym { get; private set; }
   public string Capital { get; private set; }
   public CountryName Name { get; private set; }
   public Coalition CountryCoalition { get; private set; }
   public Material Roundel { get; private set; }

   #endregion


   #region Constructor
   [ExecuteAlways]
   public void Awake()
   {
      new Dictionary<CountryName, Country>() {
         { CountryName.Gomer, new Country(CountryName.Gomer) },
         { CountryName.Scythia, new Country(CountryName.Scythia) },
         { CountryName.Madesh, new Country(CountryName.Madesh) },
         { CountryName.Javan, new Country(CountryName.Javan) },
         { CountryName.Tobolik, new Country(CountryName.Tobolik) },
         { CountryName.Meshchera, new Country(CountryName.Meshchera) },
         { CountryName.Thuras, new Country(CountryName.Thuras) },
         { CountryName.Cush, new Country(CountryName.Cush) },
         { CountryName.Mizr, new Country(CountryName.Mizr) },
         { CountryName.Huttia, new Country(CountryName.Huttia) },
         { CountryName.Palesti, new Country(CountryName.Palesti) },
         { CountryName.Elam, new Country(CountryName.Elam) },
         { CountryName.Asshuria, new Country(CountryName.Asshuria) },
         { CountryName.Arphaxia, new Country(CountryName.Arphaxia) },
         { CountryName.Lydda, new Country(CountryName.Lydda) },
         { CountryName.Aram, new Country(CountryName.Aram) }
      };
   }

   /// <summary>
   /// Create a country with a set name, demonym, capital, and coalition.
   /// </summary>
   /// <param name="country">The country to be created</param>
   public Country(CountryName country)
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
            Roundel = m_RoundelMeshchera;
            break;

         case CountryName.Thuras:
            Name = CountryName.Thuras;
            Demonym = "Thuran";
            Capital = "Thur";
            CountryCoalition = Coalition.International;
            Roundel = null;
            break;

         case CountryName.Cush:
            Name = CountryName.Cush;
            Demonym = "Cushite";
            Capital = "Ethi";
            CountryCoalition = Coalition.International;
            Roundel = null;
            break;

         case CountryName.Mizr:
            Name = CountryName.Mizr;
            Demonym = "Mizraim";
            Capital = "Mitz";
            CountryCoalition = Coalition.Cosmopolitan;
            Roundel = null;
            break;

         case CountryName.Huttia:
            Name = CountryName.Huttia;
            Demonym = "Huttite";
            Capital = "Hut";
            CountryCoalition = Coalition.Cosmopolitan;
            Roundel = null;
            break;

         case CountryName.Palesti:
            Name = CountryName.Palesti;
            Demonym = "Palesim";
            Capital = "Jebussa";
            CountryCoalition = Coalition.None;
            Roundel = null;
            break;

         case CountryName.Elam:
            Name = CountryName.Elam;
            Demonym = "Elam";
            Capital = "Eru";
            CountryCoalition = Coalition.Federal;
            Roundel = null;
            break;

         case CountryName.Asshuria:
            Name = CountryName.Asshuria;
            Demonym = "Asshur";
            Capital = "Ashkh";
            CountryCoalition = Coalition.Federal;
            Roundel = null;
            break;

         case CountryName.Arphaxia:
            Name = CountryName.Arphaxia;
            Demonym = "Arip";
            Capital = "Chalda";
            CountryCoalition = Coalition.Federal;
            Roundel = null;
            break;

         case CountryName.Lydda:
            Name = CountryName.Lydda;
            Demonym = "Sardi";
            Capital = "Lydd";
            CountryCoalition = Coalition.Joint;
            Roundel = null;
            break;

         case CountryName.Aram:
            Name = CountryName.Aram;
            Demonym = "Arami";
            Capital = "Aramecc";
            CountryCoalition = Coalition.Joint;
            Roundel = null;
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
   public Identification Relationship(Country country)
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