# AdPlatformSelector API

## 📌 Описание
Простой веб-сервис на ASP.NET Core, который позволяет:
- Загружать рекламные платформы из текстового файла
- Искать рекламные платформы по иерархической локации

Все данные хранятся **в оперативной памяти** и перезаписываются при загрузке нового файла.

## 🚀 Как запустить

1. Откройте решение в **Visual Studio 2022**
2. Установите зависимости (если нужно)
3. Убедитесь, что проект `AdPlatformSelector` выбран как стартовый
4. Нажмите **F5** или выберите **Debug > Start Debugging**
5. Перейдите в Swagger по адресу: `https://localhost:<порт>/swagger`

## 🛠 Методы API

### `POST /api/platforms/upload`
Загрузить файл с данными рекламных платформ.

**Формат файла:**
```
Платформа: /локация1,/локация2
Пример: Яндекс.Директ:/ru
```

- Каждая строка должна содержать название платформы и список локаций через запятую
- Пример:
```
Яндекс.Директ:/ru
Крутая реклама:/ru/svrd,/ru/svrd/revda
```

### `GET /api/platforms/search?location=/ru/svrd/revda`
Ищет все рекламные платформы, подходящие для указанной локации.

- Поиск идёт от самой конкретной локации вверх (до корня `/`)
- Регистр символов в локации не учитывается

## ✅ Тесты

Юнит-тесты расположены в проекте `AdPlatformSelector.Tests`

Для запуска:
- Выберите проект `AdPlatformSelector.Tests`
- Нажмите **Run All Tests** в Test Explorer

## 💡 Пример запроса

**Запрос:**
```
GET /api/platforms/search?location=/ru/svrd/revda
```

**Ответ:**
```json
["Яндекс.Директ", "Крутая реклама", "Ревдинский рабочий"]
```

## 📂 Структура решения

- `Controllers/` — API-контроллеры
- `Services/` — бизнес-логика
- `Models/` — модели данных
- `Tests/` — модульные тесты

## 🧪 Пример файла для загрузки

```
Газета уральских москвичей:/ru/msk,/ru/permobl
Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Крутая реклама:/ru/svrd
```

## 📎 Требования

- ASP.NET Core Web API
- Хранение данных в памяти (in-memory)
- Быстрый поиск платформ
- Обработка некорректного ввода
- Документация и юнит-тесты

---

💬 *По всем вопросам — welcome!*