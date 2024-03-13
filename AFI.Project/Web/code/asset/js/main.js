/********************************************************
 *                   HEADER
 ********************************************************/
function createNavOverview(href) {
  const li = document.createElement("li");
  li.className = "overview level2 cItem0 rel-level2";
  li.innerHTML = `<a title="Overview" class="overview__link" href="${href}">Overview</a>`;
  return li;
}

function navForSmallDevice() {
  const navEl = document.querySelector("#header .nav_menu nav");
  const navBar = document.querySelector("#header .nav_menu nav ul");
  const searchForm = document.querySelector(".utility-search");

  if (window.screen.width <= 991) {
    const subMenu = document.querySelectorAll("#header .level1.submenu");

    subMenu?.forEach((item) => {
      const mainAnchor = item.querySelector("a");
      mainAnchor.classList.add("mainAnchor");

      mainAnchor?.addEventListener("click", (e) => {
        e.preventDefault();
        item.classList.toggle("sub_menu_open");
      });

      // create overview - 20 Oct
      const subMenuUl = item.querySelector("ul");
      const overviewLi = subMenuUl.querySelector("li.overview");

      if (!overviewLi) {
        const overviewNewLi = createNavOverview(mainAnchor.href);
        subMenuUl.insertBefore(overviewNewLi, subMenuUl.firstChild);
      }
    });

    // Search
    searchForm?.classList.add("mobile");
    navEl?.appendChild(searchForm);
    navEl?.appendChild(navBar);
  } else {
    const topHeader = document.querySelector("#header .top-header-bg .column-splitter .row");
    topHeader?.appendChild(searchForm);
  }
}

// ******
window.addEventListener("load", () => {
  const hamburger = document.querySelector("#hamburger");

  hamburger?.addEventListener("click", () => {
    document.body.classList.toggle("screen_active");
  });

  // Remove Active
  const activeNav = document.querySelector(".level1.active");
  activeNav?.classList.remove("active");

  //  nav functionality for mobile
  navForSmallDevice();
  window.addEventListener("resize", navForSmallDevice);
});

// Remove Active
const activeNavIItem = document.querySelector("header .level1.active");
if (activeNavIItem) activeNavIItem?.classList.remove("active");

/********************************************************
 *                OTHER FUNCTIONALITY
 ********************************************************/
window.addEventListener("DOMContentLoaded", () => {
  // ******************* Footer Accordion in Mobile *******************
  const section_wrapper = document.querySelectorAll(".toggle_section_wrapper");
  section_wrapper?.forEach((parent) => {
    parent.querySelector("h2").addEventListener("click", () => {
      parent.classList.toggle("ul_open");
    });
  });

  /********************************************************
   *                  HOME CARD GROUP
   ********************************************************/
  if (screen.width <= 991) {
    const mySlider = document.querySelector(".mySlider");

    const mySwiper = mySlider?.firstElementChild;
    mySwiper?.classList.add("mySwiper");

    const swiperWrapper = mySwiper?.firstElementChild;
    swiperWrapper?.classList.remove("row");
    swiperWrapper?.classList.add("swiper-wrapper");

    // Add "swiper-slide" class in sliderItems
    //  const sliderItems = document.querySelectorAll(".mySwiper .slider_item");
    const sliderItems = document.querySelectorAll(".swiper-wrapper .col-lg-3");

    if (sliderItems) {
      sliderItems.forEach((item) => {
        item.classList.add("swiper-slide");
      });
    }

    // Pagination
    const pagination = document.createElement("div");
    pagination.classList.add("swiper-pagination");

    mySwiper?.appendChild(pagination);

    // Initiate Slider
    const swiper = new Swiper(".mySwiper", {
      spaceBetween: 30,
      // autoplay: true,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      },

      breakpoints: {
        450: {
          slidesPerView: 1,
          spaceBetween: 40,
        },
        768: {
          slidesPerView: 1,
          spaceBetween: 40,
        },
      },
    });
  }

  /********************************************************
   *                  HOME BLUE PODS
   ********************************************************/
  const bluePodItem = document.querySelector(".blue_pod_item");

  const parent_bluePods = bluePodItem?.parentElement;
  parent_bluePods?.classList.add("blue_pods_container");

  /********************************************************
   *                  ARTICLE SHARE
   ********************************************************/
  // ARTICLE PAGE SHARE FUNCTIONALITY
  const shareBadges = document.querySelector(".share-badges.js-share-badges");
  if (shareBadges) {
    shareBadges.addEventListener("click", () => shareBadges.classList.toggle("triggered"));
  }

  /********************************************************
   *                  ARTICLE SLIDER
   ********************************************************/
  var artSwiper = new Swiper(".article__mySwiper", {
    loop: true,
    autoplay: {
      delay: 4000,
      disableOnInteraction: false,
    },
  });

  /********************************************************
   *                   quote-selector_friend_form
   ********************************************************/
  var quoteSubmitBtn = document.getElementById("submit-quote");

  quoteSubmitBtn?.addEventListener("click", function (e) {
    const quoteSelectorValue = document.getElementById("quoteSelector").value;

    if (quoteSelectorValue) {
      document.getElementById("quote-selector__form").submit();
    } else {
      e.preventDefault();
      alert("Please select a quote");
    }
  });

  /********************************************************
   *         Add id to sitecore form submit button
   ********************************************************/
  const sitecoreSubmitButton = document.querySelector(".sitecore-form input[type=submit]");
  if (sitecoreSubmitButton) sitecoreSubmitButton.id = "sitecoreSubmitButton";

  /********************************************************
   *            Header and Chat conflict fix
   ********************************************************/
  const headerLeft = document.querySelector("header .logo__left");

  if (headerLeft) {
    document.querySelector("header").style.zIndex = "0";
  }

  /********************************************************
   *               HEADER SEARCH FORM SUBMIT
   ********************************************************/
  const utilitySearch = document.querySelector(".utility-search");
  const utilityInput = utilitySearch?.querySelector(".utility-search__input");

  utilitySearch?.addEventListener("submit", (e) => {
    e.preventDefault();
    const searchPageURL = "search-page#e=0&q=" + utilityInput.value;
    location.href = searchPageURL;
  });

  /********************************************************
   *               404 Page Search Functionality
   ********************************************************/
  const noPageSearchBtn = document.querySelector(".noPage__Search button.search-box-button");
  const noPageInput = document?.querySelector(".noPage__Search  input[name=textBoxSearch]");

  noPageSearchBtn?.addEventListener("click", (e) => {
    e.preventDefault();
    const searchPageURL = "search-page#e=0&q=" + noPageInput?.value;
    location.href = searchPageURL;
  });
});
