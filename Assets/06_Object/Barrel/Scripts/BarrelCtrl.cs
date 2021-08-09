using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject explosionEffect;      // ���� ȿ�� Prefab�� ������ ����
    public Mesh[] meshes;                   // ��׷��� �巳�� �޽��� ������ ����
    public Texture[] textures;              // �巳���� �ؽ�ó�� ������ �迭
    public float explosionRadius = 10.0f;   // ���� �ݰ�
    public AudioClip explostionSFX;         // ������ ����� Ŭ��

    private int hitCount = 0;               // Bullet�� ���� Ƚ��
    private Rigidbody rigidBody;            // Rigidbody Component�� ������ ����
    private MeshFilter meshFilter;          // MeshFilter Component�� ������ ����
    private MeshRenderer meshRender;        // MeshRenderer Component�� ������ ����
    private AudioSource audio;              // AudioSource Component�� ������ ����

    // Start is called before the first frame update
    private void Start()
    {
        // RigidBody Component�� ������ ����
        rigidBody = GetComponent<Rigidbody>();

        // MeshFilter Component�� ������ ����
        meshFilter = GetComponent<MeshFilter>();

        // MeshRenderer Component�� ������ ����
        meshRender = GetComponent<MeshRenderer>();

        // ������ �̿��� ������ �ؽ�ó ����
        meshRender.material.mainTexture = textures[Random.Range(0, textures.Length)];

        // AudioSource Component�� ������ ����
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ���� ������Ʈ�� �±׸� ��
        if(collision.collider.CompareTag("BULLET"))
        {
            // �Ѿ��� �浹 Ƚ���� ������Ŵ
            // 3�� �̻� ������ ExplosionBarrel ����
            if(++hitCount == 3)
            {
                ExplosionBarrel();
            }
        }
    }

    private void ExplosionBarrel()
    {
        // ���� ȿ�� �������� �������� ����
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);

        // Rigidbody Component�� mass�� 1.0���� �����Ͽ� ���Ը� ������ ����
        rigidBody.mass = 1.0f;

        // ���� ��, ���� �ڱ�ġ�� �� ����
        rigidBody.AddForce(Vector3.up * 1000.0f);

        // ���߷� ����
        IndirectDamage(transform.position);

        // ���� �߻�
        int index = Random.Range(0, meshes.Length);

        // ��׷��� �޽� ����
        meshFilter.sharedMesh = meshes[index];
        GetComponent<MeshCollider>().sharedMesh = meshes[index];

        // ������ ���
        audio.PlayOneShot(explostionSFX, 1.0f);
    }

    // ���߷��� �ֺ��� �����ϴ� �Լ�
    private void IndirectDamage(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius, 1 << 8);

        foreach(var coll in colliders)
        {
            // ���� ������ ���Ե� �巳���� Rigidbody Component ����
            var rigidbody = coll.GetComponent<Rigidbody>();

            // �巳���� mass ���� ������ ����
            rigidbody.mass = 1.0f;

            // ���߷� ����
            rigidbody.AddExplosionForce(1200.0f, position, explosionRadius, 1000.0f);
        }
    }
}