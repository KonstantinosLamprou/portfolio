<template>
 <BackButton class="hidden md:flex"/>

  <div class="max-w-3xl mx-auto py-8 px-4 md:py-12 leading-relaxed">
    <h1 class="text-3xl md:text-4xl font-bold mb-2">Nutzungsbedingungen</h1>
    <p class="text-sm italic mb-10">Zuletzt aktualisiert: Juni 2026</p>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">1. Geltungsbereich</h2>
      <p class="mb-4">
        Diese Nutzungsbedingungen gelten für den Zugriff auf und die Nutzung dieses Portfolios, des Blogs sowie der interaktiven Funktionen (Gästebuch und Kommentare). Mit der Nutzung der interaktiven Funktionen der Website erklärst du dich mit diesen Bedingungen einverstanden.
      </p>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">2. Nutzerkonten und Login</h2>
      <p class="mb-4">Für die Nutzung der Kommentar- und Gästebuchfunktionen ist ein Login über Drittanbieter (Google oder GitHub) erforderlich.</p>
      <ul class="list-disc list-outside ml-6 mb-6 space-y-2">
        <li>Du bist für alle Aktivitäten verantwortlich, die unter deinem Konto stattfinden.</li>
        <li>Der Betreiber behält sich das Recht vor, Konten bei Verstößen gegen diese Nutzungsbedingungen jederzeit und ohne Vorankündigung zu sperren oder zu löschen.</li>
      </ul>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">3. Nutzergenerierte Inhalte (Gästebuch, Kommentare & Votes)</h2>
      <p class="mb-4">
        Du hast die Möglichkeit, auf der Website Texte zu veröffentlichen und Beiträge zu bewerten. Für die von dir erstellten Inhalte bist du allein verantwortlich. Es ist strikt untersagt, Inhalte zu veröffentlichen, die:
      </p>
      <ul class="list-disc list-outside ml-6 mb-6 space-y-2">
        <li>beleidigend, rassistisch, diskriminierend oder gewaltverherrlichend sind.</li>
        <li>Urheberrechte, Markenrechte oder andere Rechte Dritter verletzen.</li>
        <li>Spam, Werbung oder schädlichen Code (z. B. Viren, Skripte) enthalten.</li>
        <li>geltendes deutsches Recht verletzen.</li>
      </ul>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">4. Hausrecht und Moderation</h2>
      <p class="mb-4">Der Betreiber übernimmt keine redaktionelle Prüfung der Kommentare oder Gästebucheinträge im Voraus.</p>
      <ul class="list-disc list-outside ml-6 mb-6 space-y-2">
        <li>Der Betreiber übt das virtuelle Hausrecht aus und behält sich das Recht vor, Kommentare, Gästebucheinträge oder Bewertungen (Votes) nach eigenem Ermessen, kommentarlos und ohne Angabe von Gründen zu löschen oder zu bearbeiten.</li>
        <li>Es besteht kein rechtlicher Anspruch auf die Veröffentlichung oder Aufrechterhaltung der von Nutzern erstellten Inhalte.</li>
      </ul>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">5. Haftungsausschluss</h2>
      <p class="mb-4">Die Nutzung der Website erfolgt auf eigene Gefahr.</p>
      <ul class="list-disc list-outside ml-6 mb-6 space-y-2">
        <li>Dieses Portfolio ist ein privates/geschäftsmäßiges Showcase-Projekt. Der Betreiber übernimmt keine Garantie für die ständige Verfügbarkeit, Fehlerfreiheit oder Datensicherheit der Website.</li>
        <li>Für den Verlust von Daten (z. B. gelöschte Kommentare oder gelöschte Nutzerkonten) wird keine Haftung übernommen.</li>
        <li>Der Betreiber haftet nicht für die Inhalte, die von Nutzern im Gästebuch oder in den Kommentaren veröffentlicht werden, macht sich diese nicht zu eigen und distanziert sich ausdrücklich von rechtswidrigen Inhalten.</li>
      </ul>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">6. Urheberrecht</h2>
      <p class="mb-4">
        Die auf dieser Website vom Betreiber veröffentlichten Inhalte (Texte, Blog-Beiträge, Code-Snippets, Grafiken) unterliegen dem deutschen Urheberrecht. Eine Vervielfältigung oder Verwendung dieser Inhalte in anderen elektronischen oder gedruckten Publikationen ist ohne ausdrückliche Zustimmung des Betreibers nicht gestattet, sofern es nicht explizit anders gekennzeichnet ist (z. B. durch eine Open-Source-Lizenz für Code-Beispiele).
      </p>
    </section>

    <section class="mb-8">
      <h2 class="text-xl md:text-2xl font-semibold mt-8 mb-4">7. Änderungen der Nutzungsbedingungen</h2>
      <p class="mb-4">
        Der Betreiber behält sich das Recht vor, diese Nutzungsbedingungen jederzeit anzupassen.
      </p>
    </section>
  </div>
</template>

<script lang="ts" setup>
import BackButton from '@/components/ui/buttons/BackButton.vue';
</script>