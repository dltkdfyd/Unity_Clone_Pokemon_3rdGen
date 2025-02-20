using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // 플레이어 이동속도
    public LayerMask solidObjectsLayer, pokemonLayer; // 레이어마스크

    private Animator _animator;
    private bool isMoving; // 이동중인지 여부
    private Vector2 input;

    public event Action OnPokemonEncountered;

    void Awake()
    {
        //_animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        // 이동 중이 아닐 때
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0) input.y = 0; // 대각선 이동 방지

            // 입력이 있을 때
            if(input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // 움직이려는 위치가 이동 가능한 위치일 때
                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos)); // 이동 시작
                }
            }
        }
    }

    // 이동 처리 코루틴
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // 부드럽게 이동 처리
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        // 이동 후 야생 포켓몬 출현 체크
        CheckForPokemon();
    }

    // 야생 포켓몬 출현 체크
    private void CheckForPokemon()
    {
        Vector2 boxSize = new Vector2(0.5f, 0.5f); // 충돌 체크 영역 크기
        Vector2 checkPosition = transform.position;
        checkPosition.y += 0.5f;

        // 현재 위치에서 포켓몬 레이어 충돌 체크
        if (Physics2D.OverlapBox(checkPosition, boxSize, 0f, pokemonLayer) != null)
        {
            // % 확률로 야생 포켓몬과 조우
            if (Random.Range(0,100) < 10)
            {
                Debug.Log("야생 포켓몬과 만났다!");
                OnPokemonEncountered();
            }
        }
    }

    // 이동 가능한 위치인지 체크
    private bool IsWalkable(Vector3 targetPos)
    {
        // 충돌 체크 영역 크기
        Vector2 boxSize = new Vector2(0.8f, 0.8f);
        Vector2 boxOffset = new Vector2(0f, 0.5f);

        // 이동 불가능한 오브젝트인지 체크
        if (Physics2D.OverlapBox(targetPos + (Vector3)boxOffset, boxSize, 0f, solidObjectsLayer) != null)
        {
            return false; // 이동 불가능
        }
        return true; // 이동 가능
    }
}