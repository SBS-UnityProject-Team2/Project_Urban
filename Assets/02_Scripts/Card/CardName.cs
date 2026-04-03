public enum CardName
{
    // [None] 1000 ~ 1999: 무속성 카드
    Punch = 1000,          // 타격
    Shooting,           // 사격
    Strike,             // 강타
    VileAttack,         // 비열한 공격
    Assault,            // 기습
    Guard,              // 방어
    Rollout,            // 구르기
    Maintenance,        // 정비
    Dummy,              // 더미
    NoneEnd,

    // [Ruin] 2000 ~ 2999: 파멸(화염) 속성
    Ignition = 2000,     // 점화
    MoltenArms,         // 융해 일격
    Embers,             // 불씨
    Inferno,            // 백염
    Backdraft,          // 백드래프트
    BlazeBarrier,       // 화염 방벽
    Reforge,            // 단련
    Incendiary,         // 소이탄
    HeatUp,             // 열기
    Overheat,           // 과열
    Cinder,             // 잔불
    Stigma,             // 낙인
    OilSplash,          // 기름 뿌리기
    RuinEnd,

    // [Psychic] 3000 ~ 3999: 초능력(얼음) 속성
    EnergyNeedle = 3000, // 에너지 송곳
    KineticGrasp,       // 염력 손아귀
    Pulse,              // 파동
    FlowArrow,          // 흐름의 화살
    GlacialWedge,       // 빙하 쐐기
    IceShield,          // 얼음 방패
    ElectricField,      // 전자기장
    AccelConcoction,    // 가속 화합물
    SuperConducter,     // 초전도체 (JSON 철자: Superconducter)
    Anxiolytic,         // 신경 안정제
    CryoPowder,         // 초저온 분말
    Disturb,            // 방해
    PsychicEnd,

    // [Bio] 4000 ~ 4999: 생체(자연) 속성
    DoubleEdge = 4000,   // 양날의 검
    Plague,             // 역병
    SpikyBush,          // 가시 덤블
    AbsorbingStrike,    // 흡수의 일격
    DistortedSlay,      // 뒤틀린 일격
    ThornWhip,          // 가시 채찍
    ElasticWall,        // 탄성 장벽
    Blooming,           // 개화
    SurgingLife,        // 맥동하는 생명
    CellChange,         // 체조직 교환
    Cycle,              // 순환
    EnfeebleSludge,     // 약화 점액
    BioEnd
}