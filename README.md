Для проверки:
Переходим в корень директории и выполняем команду:
docker-compose up --build

Если зайти на http://localhost:3000/ под пользователями, которые НЕ обладают ролью prothetic_user и нажать кнопку "Download Report" то файл отчета не скачается и будет ошибка авторизации.

Если зайти под пользователями, которые обладают ролью prothetic_user, то по нажатию кнопки "Download Report"  файл отчета будет скачан. 


Для проверки того, что используется PKCE во вкладке Network браузера, можно посмотреть запрос http://localhost:8080/realms/reports-realm/protocol/openid-connect/auth и на вкладке Payload будут видны параметры code_challenge и code_challenge_method.