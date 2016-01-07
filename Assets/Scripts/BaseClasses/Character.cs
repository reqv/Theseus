using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Enumerator mówiący o statusie postaci względem efektów kontroli tłumu
/// </summary>
public enum Status
{
    OK = 0,
    Slowed = 1,
    Blinded = 2,
    Stunned = 3
}

/**
 * <summary>
 * 	Abstrakcyjna klasa bazowa dla postaci uczestniczących w systemie walki
 * </summary>
 * <remarks>
 * 	Zawiera podstawowe pola i metody użyteczne w systemie walki.
 * </remarks>
 */
public abstract class Character : TheseusGameObject
{
    /// <summary>
    /// 	Aktualny status "kontroli tłumu" na postaci
    /// </summary>
    protected Status _status = Status.OK;

    /// <summary>
    /// 	Zegar odliczający czas efektów "kontroli tłumu"
    /// </summary>
    protected double _ccEffectTimer;

    /// <summary>
    /// 	Zmienna sprawdzająca, czy obiekt otrzymuje obrażenia w czasie
    /// </summary>
    protected bool _isDamagedOverTime = false;

    /// <summary>
    /// 	Zegar odliczający czas po którym obrażenia czasowe znikają
    /// </summary>
    protected double _DOTTimer;

    /// <summary>
    /// 	Zegar odliczający czas do naliczenia obrażeń czasowych
    /// </summary>
    protected int _lastDoTTick;

    /// <summary>
    /// 	Wielkość obrażeń zadawanych w czasię ( co sekundę )
    /// </summary>
    protected int _DOTDamage;

    [Tooltip("Aktualne zdrowie potwora")]
    [SerializeField]
    /// <summary>
    /// 	Parametr trzymajacy aktualne zdrowie postaci
    /// </summary>
    protected int _healthPoints;

    [Tooltip("Siła ataku potwora")]
    [SerializeField]
    /// <summary>
    /// 	Parametr trzymajacy siłę ataku postaci
    /// </summary>
    protected int _attackPower;

    [Tooltip("Prędkość lotu pocisków")]
    [SerializeField]
    /// <summary>
    /// 	Parametr mówiący o prędkości wystrzeliwanych pocisków, ważny tylko dla jednostek strzelających.
    /// </summary>
    protected int _bulletVelocityFactor;

    [Tooltip("Maksymalne przyspieszenie obiektu.")]
    [SerializeField]
    /// <summary>
    /// 	Parametr serializowany, określa maksymalną szybkość obiektu
    /// </summary>
    protected float _maxSpeed;

    /// <summary>
    /// 	Prawdziwe przyśpieszenie obiektu dla której podstawą jest _maxSpeed, zależna również od innych czynników 
    /// </summary>
    protected float _realMaxSpeed;

    [Tooltip("Zasięg spostrzeżenia gracza przez obiekt.")]
    [SerializeField]
    /// <summary>
    /// 	Parametr serializowany, określa zasięg widzenia innych obiektów przez dany obiekt
    /// </summary>
    protected float _range;

    /// <summary>
    /// 	Prawdziwa wartość spostrzegawczości potwora dla której podstawą jest _range, zależna również od innych czynników 
    /// </summary>
    protected float _realRange;

    [Tooltip("Zasięg od którego obiekt rozpoczyna atak.")]
    [SerializeField]
    /// <summary>
    /// 	Parametr serializowany, określa maksymalną szybkość obiektu
    /// </summary>
    protected float _attackDistance;

    /// <summary>
    /// 	Prawdziwa wartość zasięgu ataku potwora dla której wartością bazową jest _attackDistance, zależna również od innych czynników 
    /// </summary>
    protected float _realAttackDistance;

    /// <summary>
    /// 	Metoda uruchamiana, gdy postać odnosi obrażenia
    /// </summary>
    /// <param name="damage">Określa ilość obrażeń jakie zostały zadane</param>
    /// <remarks>
    /// 	Podana zmienna oznacza ilość obrażeń odejmowanych od aktualnego zdrowia postaci, jeżeli postać posiada zerową lub ujemną ilość punktów życia uruchamiana jest metoda Die().
    /// </remarks>
    public virtual void TakingDamage(int damage)
    {
        _healthPoints -= damage;
        if (_healthPoints <= 0)
            Die();
    }

    /// <summary>
    /// 	Metoda aktualizująca status postaci wzgledem czasu
    /// </summary>
    /// <remarks>
    /// 	Metoda sprawdza aktualny stan postaci. Jeżeli postać wyszła (wedle zegara efektów kontroli tłumu) z efektu kontroli tłumu to
    /// 	metoda ta wyłącza wszystkie negatywne efekty tego statusu. Metoda ta odpowiada również za naliczanie obrażeń czasowych i ich zdejmowanie.
    /// </remarks>
    protected void CheckPersonalStatus()
    {
        if (_status != Status.OK)
            if (_ccEffectTimer <= 0)
            {
                _status = Status.OK;
                _realMaxSpeed = _maxSpeed;
                _realAttackDistance = _attackDistance;
                _realRange = _range;
            }
            else
                _ccEffectTimer -= Time.deltaTime;

        if (_isDamagedOverTime)
            if (_DOTTimer <= 0)
            {
                _isDamagedOverTime = false;
                _Render2D.color = Color.white;
            }
            else
            {
                _DOTTimer -= Time.deltaTime;
                if ((int)_DOTTimer != _lastDoTTick)
                {
                    TakingDamage(_DOTDamage);
                    _lastDoTTick = (int)_DOTTimer;
                }
            }
    }

    /// <summary>
    /// 	Metoda publiczna pozwalająca na ustawienie efektu kontroli tłumu dla postaci
    /// </summary>
    /// <param name="status">Status jaki zostanie nałożony na postać(patrz 'public enum Status').</param>
    /// <param name="time">Czas podczas którego efekt będzie aktywny(podany w sekundach).</param>
    public virtual void SetCrowdControl(Status status, int time)
    {
        if (_status <= status)
        {
            switch (status)
            {
                case Status.Slowed:
                    if (_status != Status.Slowed)
                        _realMaxSpeed = _maxSpeed / 2;
                    break;
                case Status.Blinded:
                    _realAttackDistance = 1;
                    _realRange = 3;
                    break;
                default: break;
            }
            _status = status;
            _ccEffectTimer = time;
        }
    }

    /// <summary>
    /// 	Metoda publiczna pozwalająca na ustawienie obrażeń zadawanych w czasię
    /// </summary>
    /// <param name="time">Czas podczas którego efekt będzie aktywny(podany w sekundach).</param>
    /// <param name="damage">Ilość obrażeń zadawanych co sekundę.</param>
    public virtual void SetDamageOverTime(int time, int damage)
    {
        if (_isDamagedOverTime)
        {
            if (damage > _DOTDamage)
                _DOTDamage = damage;
        }
        else
        {
            _DOTTimer = time;
            _lastDoTTick = (int)_DOTTimer;
            _DOTDamage = damage;
            _isDamagedOverTime = true;
        }
        _Render2D.color = Color.green;
    }

    /// <summary>
    /// 	Metoda uruchamiana, gdy postać umiera
    /// </summary>
    protected abstract void Die();
}
