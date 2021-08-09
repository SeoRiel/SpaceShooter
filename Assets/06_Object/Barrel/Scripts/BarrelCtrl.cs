using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject explosionEffect;      // 폭발 효과 Prefab을 저장할 변수
    public Mesh[] meshes;                   // 찌그러진 드럼통 메쉬를 저장할 변수
    public Texture[] textures;              // 드럼통의 텍스처를 저장할 배열
    public float explosionRadius = 10.0f;   // 폭발 반경
    public AudioClip explostionSFX;         // 폭발음 오디오 클립

    private int hitCount = 0;               // Bullet에 맞은 횟수
    private Rigidbody rigidBody;            // Rigidbody Component를 저장할 변수
    private MeshFilter meshFilter;          // MeshFilter Component를 저장할 변수
    private MeshRenderer meshRender;        // MeshRenderer Component를 저장할 변수
    private AudioSource audio;              // AudioSource Component를 저장할 변수

    // Start is called before the first frame update
    private void Start()
    {
        // RigidBody Component를 추출해 저장
        rigidBody = GetComponent<Rigidbody>();

        // MeshFilter Component를 추출해 저장
        meshFilter = GetComponent<MeshFilter>();

        // MeshRenderer Component를 추출해 저장
        meshRender = GetComponent<MeshRenderer>();

        // 난수를 이용한 무작위 텍스처 적용
        meshRender.material.mainTexture = textures[Random.Range(0, textures.Length)];

        // AudioSource Component를 저장할 변수
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 게임 오브젝트의 태그를 비교
        if(collision.collider.CompareTag("BULLET"))
        {
            // 총알의 충돌 횟수를 증가시킴
            // 3발 이상 맞으면 ExplosionBarrel 실행
            if(++hitCount == 3)
            {
                ExplosionBarrel();
            }
        }
    }

    private void ExplosionBarrel()
    {
        // 폭발 효과 프리팹을 동적으로 생성
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);

        // Rigidbody Component의 mass를 1.0으로 설정하여 무게를 가볍게 만듬
        rigidBody.mass = 1.0f;

        // 폭발 시, 위로 솟구치는 힘 적용
        rigidBody.AddForce(Vector3.up * 1000.0f);

        // 폭발력 생성
        IndirectDamage(transform.position);

        // 난수 발생
        int index = Random.Range(0, meshes.Length);

        // 찌그러진 메쉬 적용
        meshFilter.sharedMesh = meshes[index];
        GetComponent<MeshCollider>().sharedMesh = meshes[index];

        // 폭발음 출력
        audio.PlayOneShot(explostionSFX, 1.0f);
    }

    // 폭발력을 주변에 전달하는 함수
    private void IndirectDamage(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius, 1 << 8);

        foreach(var coll in colliders)
        {
            // 폭발 범위에 포함된 드럼통의 Rigidbody Component 추출
            var rigidbody = coll.GetComponent<Rigidbody>();

            // 드럼통의 mass 값을 가볍게 설정
            rigidbody.mass = 1.0f;

            // 폭발력 전달
            rigidbody.AddExplosionForce(1200.0f, position, explosionRadius, 1000.0f);
        }
    }
}