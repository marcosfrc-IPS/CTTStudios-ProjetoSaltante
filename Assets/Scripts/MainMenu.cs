using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel, infoPanel, settingsPanel;
    //Cria um metodo que chama os painéis e permite mostrar os diferentes menus de informações, configurações e o menu principal.
    public void ShowMain()     => Show(mainPanel);
    public void ShowInfo()     => Show(infoPanel);
    public void ShowSettings() => Show(settingsPanel);
    //Ativa apenas um painel de cada vez, desativando os outros.
    void Show(GameObject target)
    {
        mainPanel.SetActive(target == mainPanel);
        infoPanel.SetActive(target == infoPanel);
        settingsPanel.SetActive(target == settingsPanel);
    }
    //Cria um metodo que permite sair do jogo.
    public void Quit() => Application.Quit();
}