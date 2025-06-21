# AntiAdminRaid

![SCP:SL Plugin](https://img.shields.io/badge/SCP--SL%20Plugin-blue?style=for-the-badge)
![Language](https://img.shields.io/badge/Language-C%23-blueviolet?style=for-the-badge)
![Downloads](https://img.shields.io/github/downloads/angelseraphim/AntiAdminRaid/total?label=Downloads&color=333333&style=for-the-badge)

---
**AntiAdminRaid** is a plugin for SCP: Secret Laboratory using **LabApi**, designed to automatically prevent mass banning raids by rogue administrators.
---

## ⚙️ Configuration (`config.yml`)

```yaml
debug: false
ban_count: 3
ban_count_k_d: 60
un_ban_players: true
simultaneous_bans_count: 3
raid_reason: 'Ugh...'
raider_ban_duration: 7
web_hook: ''
web_hook_text: '**%nick%** (%steam%)[%ip%] :name_badge: has been banned for suspected admin abuse.'
```

### Parameter Descriptions:

* `ban_count` — Number of bans an admin must issue before being flagged and banned.
* `ban_count_k_d` — Time (in seconds) after which the ban counter resets.
* `un_ban_players` — If `true`, all players banned by the suspected admin will be automatically unbanned.
* `simultaneous_bans_count` — Maximum number of players an admin can ban **simultaneously** (e.g., using multi-select in the admin panel).
* `raid_reason` — Reason used when banning the suspected admin.
* `raider_ban_duration` — Duration of the admin’s ban (in days).
* `web_hook` — Discord WebHook URL for sending notifications.
* `web_hook_text` — Message sent to the WebHook when an admin is banned.

### WebHook Variables:

* `%nick%` — Admin's nickname.
* `%steam%` — Admin's SteamID64.
* `%ip%` — Admin's IP address.
* `%name_badge%` — Admin's group or rank.
