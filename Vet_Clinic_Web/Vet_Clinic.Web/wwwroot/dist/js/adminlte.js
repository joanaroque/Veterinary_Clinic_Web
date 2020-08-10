/*! AdminLTE app.js
* ================
* Main JS application file for AdminLTE v2. This file
* should be included in all pages. It controls some layout
* options and implements exclusive AdminLTE plugins.
*
* @Author  Almsaeed Studio
* @Support <https://www.almsaeedstudio.com>
* @Email   <abdullah@almsaeedstudio.com>
* @version 2.4.8
* @repository git://github.com/almasaeed2010/AdminLTE.git
* @license MIT <http://opensource.org/licenses/MIT>
*/

// Make sure jQuery has been loaded
if (typeof jQuery === 'undefined') {
throw new Error('AdminLTE requires jQuery')
}

/* BoxRefresh()
 * =========
 * Adds AJAX content control to a box.
 *
 * @Usage: $('#my-box').boxRefresh(options)
 *         or add [data-widget="box-refresh"] to the box element
 *         Pass any option as data-option="value"
 */

/*
PAGE WITH BEAR NOT FOUND ************************************************************************************************************
*/
const {
    React: { useEffect },
    ReactDOM: { render },
    styled: { default: styled },
} = window

const GLITCH_CHARS = '`΅™£Ά?§¶•ªΊ–?εί?ƒ©???¬…ζ?η??µ??χ/????<>/'.split('')

const ReadableChar = styled.span``

const GlitchyChar = styled.span``
/**
 * A glitchy text reveal title
 */
const GlitchyText = ({ children, ...props }) => {
    return (
        <h1 {...props} className={`glitchy-text ${props.className}`}>
            <ReadableChar className="glitchy-text__char--readable">
                {children}
            </ReadableChar>
            {children.split('').map((char, idx) => {
                const charStyle = {
                    '--count': Math.random() * 5 + 1,
                }
                for (let i = 0; i < 10; i++) {
                    charStyle[`--char-${i}`] = `"${
                        GLITCH_CHARS[Math.floor(Math.random() * GLITCH_CHARS.length)]
                        }"`
                }
                return (
                    <GlitchyChar
                        className="glitchy-text__char"
                        aria-hidden={true}
                        data-char={char}
                        key={`glitch-char--${idx}`}
                        style={charStyle}>
                        {char}
                    </GlitchyChar>
                )
            })}
        </h1>
    )
}

const Logo = ({ className }) => {
    return (
        <svg
            className={`bear-logo ${className}`}
            viewBox="0 0 300 300"
            xmlns="http://www.w3.org/2000/svg">
            <g transform="matrix(1.34105 0 0 1.34105 -51.157 -1049.694)">
                <path
                    d="M242.822 893.869c0 61.251-41.365 106.284-94.67 106.284-53.306 0-90.974-45.033-90.974-106.284 0-61.252 37.668-85.951 90.973-85.951 53.306 0 94.67 24.699 94.67 85.95z"
                    fill="#803300"
                />
                <path
                    d="M211.925 958.105c0 19.907-28.138 38.819-62.85 38.819-34.71 0-61-18.912-61-38.82 0-19.907 26.29-33.273 61-33.273 34.712 0 62.85 13.366 62.85 33.274z"
                    fill="#e9c6af"
                />
                <path d="M179.114 931.763c0 7.657-19.04 24.493-30.27 24.493s-32.117-16.836-32.117-24.493 20.888-12.477 32.118-12.477 30.27 4.82 30.27 12.477z" />
                <ellipse
                    ry="23.111"
                    rx="23.762"
                    cy="827.682"
                    cx="68.304"
                    fill="#803300"
                />
                <path
                    d="M84.784 826.317a16.549 16.095 0 00-16.48-14.731 16.549 16.095 0 00-16.548 16.095 16.549 16.095 0 0016.548 16.096 16.549 16.095 0 001.132-.039c.815-1.337 1.582-2.727 2.471-3.983a65.703 65.703 0 015.055-6.283 65.597 65.597 0 015.713-5.548c.668-.576 1.417-1.058 2.109-1.607z"
                    fill="#e9c6af"
                />
                <ellipse
                    transform="scale(-1 1)"
                    cx="-231.243"
                    cy="827.682"
                    rx="23.762"
                    ry="23.111"
                    fill="#803300"
                />
                <path
                    d="M214.764 826.317a16.549 16.095 0 0116.48-14.731 16.549 16.095 0 0116.548 16.095 16.549 16.095 0 01-16.549 16.096 16.549 16.095 0 01-1.131-.039c-.816-1.337-1.582-2.727-2.472-3.983a65.703 65.703 0 00-5.055-6.283 65.597 65.597 0 00-5.712-5.548c-.67-.576-1.418-1.058-2.11-1.607z"
                    fill="#e9c6af"
                />
                <path
                    d="M147.731 815.358c-13.396 0-26.022 1.079-37.671 3.334v11.353c11.649-2.255 24.275-3.334 37.671-3.334 15.104 0 29.459 1.373 42.667 4.256v-11.355c-13.208-2.883-27.564-4.254-42.667-4.254z"
                    fill="red"
                />
                <path
                    d="M165.195 816.013a15.875 7.813 0 014.43 5.412 15.875 7.813 0 01-5.414 5.863c9.116.653 17.878 1.866 26.186 3.68v-11.356c-8.007-1.748-16.44-2.931-25.202-3.6z"
                    fill="#e50000"
                />
                <path
                    d="M148.387 789.038c-43.49 0-75.817 18.34-83.229 62.715 12.322-10.516 27.524-17.553 44.903-21.51 0 0-2.734-13.778 10.25-16.029 12.983-2.25 14.025-4 27.421-4 15.103 0 16.422.44 30.666 4 14.245 3.561 12 17.113 12 17.113 17.218 4.421 32.483 11.864 44.9 22.71-7.275-46.03-42.684-64.999-86.911-64.999z"
                    fill="#1a1a1a"
                />
                <path
                    className="bear__tear-stream"
                    d="M190.665 893.336v96.221c8.24-4.43 15.761-10.15 22.37-16.971v-79.25h-22.37zM86.056 893.336v80.399c6.546 6.885 14.056 12.592 22.37 16.92v-97.32h-22.37z"
                />
                <path
                    className="bear__brows"
                    d="M96.601 864.041a21.271 21.271 0 01-6.78 15.572 21.271 21.271 0 01-16.02 5.644M203.53 864.041a21.271 21.271 0 006.78 15.572 21.271 21.271 0 0016.02 5.644"
                    fill="none"
                    strokeWidth="2.983"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                />
                <circle
                    className="bear__eye bear__eye--left"
                    r="11.185"
                    cy="894.081"
                    cx="97.242"
                />
                <circle
                    className="bear__eye bear__eye--right"
                    r="11.185"
                    cx="201.851"
                    cy="894.081"
                />
                <g className="bear__shades">
                    <path d="M77.29 854.608c-6.508-.07-13.363.182-22.8 2.131l-14.318 2.769 2.308 15.038c2.094.293 4.984 1.016 6.352 2.848 3.528 4.722 1.202 9.286 2.003 17.625 2.67 27.781 11.92 33.596 26.247 35.267 7.502.875 22.816 2.282 33.28-1.433 9.456-3.357 18.223-9.868 24.158-17.96 6.274-8.553 6.948-18.43 10.843-25.92 2.76-5.31 7.7-3.786 9.258.39 2.951 7.91 4.379 16.977 10.653 25.53 5.935 8.092 14.702 14.603 24.159 17.96 10.463 3.715 25.777 2.308 33.279 1.433 14.326-1.671 23.578-7.486 26.249-35.267.801-8.339-1.525-12.903 2.003-17.625 1.414-1.893 4.463-2.611 6.567-2.885l2.297-14.962-14.522-2.808c-15.1-3.12-23.591-1.89-34.623-1.957-4.093-.025-8.777.115-13.136.573-8.125.855-16.176 2.382-24.174 4.05-7.988 1.665-15.589 6.1-23.747 5.942-7.868-.153-14.966-4.977-22.664-6.616-8.132-1.732-14.824-3.106-24.713-3.376-4.794-.13-9.043-.598-13.136-.573-4.137.026-7.918-.132-11.823-.174z" />
                    <path
                        d="M85.28 860.471c-10.088.168-24.807.706-28.48 12.251-3.277 15.249-3.122 32.76 5.735 45.627 7.396 10.745 23.365 8.637 35.258 8.22 18.899-.663 35.138-15.383 40.158-33.389 1.93-7.753 3.906-11.697.696-19.382-7.653-8.953-20.099-11.778-30.58-12.967a443.553 443.553 0 00-22.787-.36z"
                        fill="#333"
                    />
                    <path
                        d="M56.04 864.299a4.079 1.692 0 01-4.058 1.692 4.079 1.692 0 01-4.1-1.675 4.079 1.692 0 014.017-1.71 4.079 1.692 0 014.14 1.658"
                        fill="#b3b3b3"
                    />
                    <path
                        d="M214.515 860.471c10.088.168 24.807.706 28.481 12.251 3.276 15.249 3.12 32.76-5.736 45.627-7.396 10.745-23.365 8.637-35.257 8.22-18.9-.663-35.139-15.383-40.16-33.389-1.928-7.753-3.905-11.697-.695-19.382 7.653-8.953 20.1-11.778 30.58-12.967a443.553 443.553 0 0122.787-.36z"
                        fill="#333"
                    />
                    <path
                        d="M243.755 864.299a4.079 1.692 0 004.058 1.692 4.079 1.692 0 004.1-1.675 4.079 1.692 0 00-4.017-1.71 4.079 1.692 0 00-4.14 1.658"
                        fill="#b3b3b3"
                    />
                    <path
                        d="M100.812 860.598l-19.186 66.305c5.485.302 11.181-.16 16.165-.335.244-.008.486-.03.73-.043l18.585-64.222c-3.068-.678-6.13-1.14-9.039-1.47a473.891 473.891 0 00-7.255-.235zM89.154 860.456c-1.292.002-2.586 0-3.875.014-1.081.018-2.235.05-3.403.09l-17.662 59.853c1.729 1.827 3.77 3.172 6.025 4.148l18.915-64.105z"
                        fill="#fff"
                    />
                </g>
            </g>
        </svg>
    )
}

const Container = styled.div
  display: inline-grid;
  grid-gap: 0 1rem;
  grid-template-columns: auto 1fr;
  grid-template-rows: auto auto;


const Code = styled(GlitchyText)
  align-self: end;
  font-size: 3rem;
  @media (min-width: 768px) {
    font-size: 6rem;
  


const AppShell = styled.main
  display: flex;
  align-items: center;
  justify-content: center;


const CodeMessage = styled(GlitchyText)
  font-size: 1.5rem;

  @media (min-width: 768px) {
    font-size: 3rem;
 


const Message = styled.a
  grid-column: span 2;
  grid-row: -1;
  text-align: center;
  margin: 2rem auto;

  &:hover {
    & ~ img {
      background: red;
    }
  }


const FourOhFour = () => {
    useEffect(() => {
        const root = document.documentElement
        const update = e => {
            if (e.acceleration && e.acceleration.x !== null) {
                root.style.setProperty('--X', e.acceleration.x)
                root.style.setProperty('--Y', e.acceleration.y)
            } else {
                root.style.setProperty('--X', e.pageX / window.innerWidth - 0.5)
                root.style.setProperty('--Y', e.pageY / window.innerHeight - 0.5)
            }
        }

        document.body.addEventListener('mousemove', update)
        window.ondevicemotion = update
        return () => {
            document.body.removeEventListener('mousemove', update)
        }
    }, [])
    return (
        <AppShell>
            <Container>
                <Message className="return-link" href="/#" rel="noreferrer noopener">
                    Return to happiness
        </Message>
                <Logo className="bear-logo--tears" />
                <Code className="four-oh-four__code">404</Code>
                <CodeMessage className="four-oh-four__code-message">
                    Not found
        </CodeMessage>
            </Container>
        </AppShell>
    )
}
const ROOT = document.getElementById('app')
render(<FourOhFour />, ROOT)


/*
PAGE WITH BEAR NOT FOUND ************************************************************************************************************
*/

+function ($) {
  'use strict';

  var DataKey = 'lte.boxrefresh';

  var Default = {
    source         : '',
    params         : {},
    trigger        : '.refresh-btn',
    content        : '.box-body',
    loadInContent  : true,
    responseType   : '',
    overlayTemplate: '<div class="overlay"><div class="fa fa-refresh fa-spin"></div></div>',
    onLoadStart    : function () {
    },
    onLoadDone     : function (response) {
      return response;
    }
  };

  var Selector = {
    data: '[data-widget="box-refresh"]'
  };

  // BoxRefresh Class Definition
  // =========================
  var BoxRefresh = function (element, options) {
    this.element  = element;
    this.options  = options;
    this.$overlay = $(options.overlay);

    if (options.source === '') {
      throw new Error('Source url was not defined. Please specify a url in your BoxRefresh source option.');
    }

    this._setUpListeners();
    this.load();
  };

  BoxRefresh.prototype.load = function () {
    this._addOverlay();
    this.options.onLoadStart.call($(this));

    $.get(this.options.source, this.options.params, function (response) {
      if (this.options.loadInContent) {
        $(this.options.content).html(response);
      }
      this.options.onLoadDone.call($(this), response);
      this._removeOverlay();
    }.bind(this), this.options.responseType !== '' && this.options.responseType);
  };

  // Private

  BoxRefresh.prototype._setUpListeners = function () {
    $(this.element).on('click', Selector.trigger, function (event) {
      if (event) event.preventDefault();
      this.load();
    }.bind(this));
  };

  BoxRefresh.prototype._addOverlay = function () {
    $(this.element).append(this.$overlay);
  };

  BoxRefresh.prototype._removeOverlay = function () {
    $(this.element).remove(this.$overlay);
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, (data = new BoxRefresh($this, options)));
      }

      if (typeof data == 'string') {
        if (typeof data[option] == 'undefined') {
          throw new Error('No method named ' + option);
        }
        data[option]();
      }
    });
  }

  var old = $.fn.boxRefresh;

  $.fn.boxRefresh             = Plugin;
  $.fn.boxRefresh.Constructor = BoxRefresh;

  // No Conflict Mode
  // ================
  $.fn.boxRefresh.noConflict = function () {
    $.fn.boxRefresh = old;
    return this;
  };

  // BoxRefresh Data API
  // =================
  $(window).on('load', function () {
    $(Selector.data).each(function () {
      Plugin.call($(this));
    });
  });

}(jQuery);


/* BoxWidget()
 * ======
 * Adds box widget functions to boxes.
 *
 * @Usage: $('.my-box').boxWidget(options)
 *         This plugin auto activates on any element using the `.box` class
 *         Pass any option as data-option="value"
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.boxwidget';

  var Default = {
    animationSpeed : 500,
    collapseTrigger: '[data-widget="collapse"]',
    removeTrigger  : '[data-widget="remove"]',
    collapseIcon   : 'fa-minus',
    expandIcon     : 'fa-plus',
    removeIcon     : 'fa-times'
  };

  var Selector = {
    data     : '.box',
    collapsed: '.collapsed-box',
    header   : '.box-header',
    body     : '.box-body',
    footer   : '.box-footer',
    tools    : '.box-tools'
  };

  var ClassName = {
    collapsed: 'collapsed-box'
  };

  var Event = {
    collapsed: 'collapsed.boxwidget',
    expanded : 'expanded.boxwidget',
    removed  : 'removed.boxwidget'
  };

  // BoxWidget Class Definition
  // =====================
  var BoxWidget = function (element, options) {
    this.element = element;
    this.options = options;

    this._setUpListeners();
  };

  BoxWidget.prototype.toggle = function () {
    var isOpen = !$(this.element).is(Selector.collapsed);

    if (isOpen) {
      this.collapse();
    } else {
      this.expand();
    }
  };

  BoxWidget.prototype.expand = function () {
    var expandedEvent = $.Event(Event.expanded);
    var collapseIcon  = this.options.collapseIcon;
    var expandIcon    = this.options.expandIcon;

    $(this.element).removeClass(ClassName.collapsed);

    $(this.element)
      .children(Selector.header + ', ' + Selector.body + ', ' + Selector.footer)
      .children(Selector.tools)
      .find('.' + expandIcon)
      .removeClass(expandIcon)
      .addClass(collapseIcon);

    $(this.element).children(Selector.body + ', ' + Selector.footer)
      .slideDown(this.options.animationSpeed, function () {
        $(this.element).trigger(expandedEvent);
      }.bind(this));
  };

  BoxWidget.prototype.collapse = function () {
    var collapsedEvent = $.Event(Event.collapsed);
    var collapseIcon   = this.options.collapseIcon;
    var expandIcon     = this.options.expandIcon;

    $(this.element)
      .children(Selector.header + ', ' + Selector.body + ', ' + Selector.footer)
      .children(Selector.tools)
      .find('.' + collapseIcon)
      .removeClass(collapseIcon)
      .addClass(expandIcon);

    $(this.element).children(Selector.body + ', ' + Selector.footer)
      .slideUp(this.options.animationSpeed, function () {
        $(this.element).addClass(ClassName.collapsed);
        $(this.element).trigger(collapsedEvent);
      }.bind(this));
  };

  BoxWidget.prototype.remove = function () {
    var removedEvent = $.Event(Event.removed);

    $(this.element).slideUp(this.options.animationSpeed, function () {
      $(this.element).trigger(removedEvent);
      $(this.element).remove();
    }.bind(this));
  };

  // Private

  BoxWidget.prototype._setUpListeners = function () {
    var that = this;

    $(this.element).on('click', this.options.collapseTrigger, function (event) {
      if (event) event.preventDefault();
      that.toggle($(this));
      return false;
    });

    $(this.element).on('click', this.options.removeTrigger, function (event) {
      if (event) event.preventDefault();
      that.remove($(this));
      return false;
    });
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, (data = new BoxWidget($this, options)));
      }

      if (typeof option == 'string') {
        if (typeof data[option] == 'undefined') {
          throw new Error('No method named ' + option);
        }
        data[option]();
      }
    });
  }

  var old = $.fn.boxWidget;

  $.fn.boxWidget             = Plugin;
  $.fn.boxWidget.Constructor = BoxWidget;

  // No Conflict Mode
  // ================
  $.fn.boxWidget.noConflict = function () {
    $.fn.boxWidget = old;
    return this;
  };

  // BoxWidget Data API
  // ==================
  $(window).on('load', function () {
    $(Selector.data).each(function () {
      Plugin.call($(this));
    });
  });
}(jQuery);


/* ControlSidebar()
 * ===============
 * Toggles the state of the control sidebar
 *
 * @Usage: $('#control-sidebar-trigger').controlSidebar(options)
 *         or add [data-toggle="control-sidebar"] to the trigger
 *         Pass any option as data-option="value"
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.controlsidebar';

  var Default = {
    slide: true
  };

  var Selector = {
    sidebar: '.control-sidebar',
    data   : '[data-toggle="control-sidebar"]',
    open   : '.control-sidebar-open',
    bg     : '.control-sidebar-bg',
    wrapper: '.wrapper',
    content: '.content-wrapper',
    boxed  : '.layout-boxed'
  };

  var ClassName = {
    open : 'control-sidebar-open',
    fixed: 'fixed'
  };

  var Event = {
    collapsed: 'collapsed.controlsidebar',
    expanded : 'expanded.controlsidebar'
  };

  // ControlSidebar Class Definition
  // ===============================
  var ControlSidebar = function (element, options) {
    this.element         = element;
    this.options         = options;
    this.hasBindedResize = false;

    this.init();
  };

  ControlSidebar.prototype.init = function () {
    // Add click listener if the element hasn't been
    // initialized using the data API
    if (!$(this.element).is(Selector.data)) {
      $(this).on('click', this.toggle);
    }

    this.fix();
    $(window).resize(function () {
      this.fix();
    }.bind(this));
  };

  ControlSidebar.prototype.toggle = function (event) {
    if (event) event.preventDefault();

    this.fix();

    if (!$(Selector.sidebar).is(Selector.open) && !$('body').is(Selector.open)) {
      this.expand();
    } else {
      this.collapse();
    }
  };

  ControlSidebar.prototype.expand = function () {
    if (!this.options.slide) {
      $('body').addClass(ClassName.open);
    } else {
      $(Selector.sidebar).addClass(ClassName.open);
    }

    $(this.element).trigger($.Event(Event.expanded));
  };

  ControlSidebar.prototype.collapse = function () {
    $('body, ' + Selector.sidebar).removeClass(ClassName.open);
    $(this.element).trigger($.Event(Event.collapsed));
  };

  ControlSidebar.prototype.fix = function () {
    if ($('body').is(Selector.boxed)) {
      this._fixForBoxed($(Selector.bg));
    }
  };

  // Private

  ControlSidebar.prototype._fixForBoxed = function (bg) {
    bg.css({
      position: 'absolute',
      height  : $(Selector.wrapper).height()
    });
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, (data = new ControlSidebar($this, options)));
      }

      if (typeof option == 'string') data.toggle();
    });
  }

  var old = $.fn.controlSidebar;

  $.fn.controlSidebar             = Plugin;
  $.fn.controlSidebar.Constructor = ControlSidebar;

  // No Conflict Mode
  // ================
  $.fn.controlSidebar.noConflict = function () {
    $.fn.controlSidebar = old;
    return this;
  };

  // ControlSidebar Data API
  // =======================
  $(document).on('click', Selector.data, function (event) {
    if (event) event.preventDefault();
    Plugin.call($(this), 'toggle');
  });

}(jQuery);


/* DirectChat()
 * ===============
 * Toggles the state of the control sidebar
 *
 * @Usage: $('#my-chat-box').directChat()
 *         or add [data-widget="direct-chat"] to the trigger
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.directchat';

  var Selector = {
    data: '[data-widget="chat-pane-toggle"]',
    box : '.direct-chat'
  };

  var ClassName = {
    open: 'direct-chat-contacts-open'
  };

  // DirectChat Class Definition
  // ===========================
  var DirectChat = function (element) {
    this.element = element;
  };

  DirectChat.prototype.toggle = function ($trigger) {
    $trigger.parents(Selector.box).first().toggleClass(ClassName.open);
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        $this.data(DataKey, (data = new DirectChat($this)));
      }

      if (typeof option == 'string') data.toggle($this);
    });
  }

  var old = $.fn.directChat;

  $.fn.directChat             = Plugin;
  $.fn.directChat.Constructor = DirectChat;

  // No Conflict Mode
  // ================
  $.fn.directChat.noConflict = function () {
    $.fn.directChat = old;
    return this;
  };

  // DirectChat Data API
  // ===================
  $(document).on('click', Selector.data, function (event) {
    if (event) event.preventDefault();
    Plugin.call($(this), 'toggle');
  });

}(jQuery);


/* Layout()
 * ========
 * Implements AdminLTE layout.
 * Fixes the layout height in case min-height fails.
 *
 * @usage activated automatically upon window load.
 *        Configure any options by passing data-option="value"
 *        to the body tag.
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.layout';

  var Default = {
    slimscroll : true,
    resetHeight: true
  };

  var Selector = {
    wrapper       : '.wrapper',
    contentWrapper: '.content-wrapper',
    layoutBoxed   : '.layout-boxed',
    mainFooter    : '.main-footer',
    mainHeader    : '.main-header',
    sidebar       : '.sidebar',
    controlSidebar: '.control-sidebar',
    fixed         : '.fixed',
    sidebarMenu   : '.sidebar-menu',
    logo          : '.main-header .logo'
  };

  var ClassName = {
    fixed         : 'fixed',
    holdTransition: 'hold-transition'
  };

  var Layout = function (options) {
    this.options      = options;
    this.bindedResize = false;
    this.activate();
  };

  Layout.prototype.activate = function () {
    this.fix();
    this.fixSidebar();

    $('body').removeClass(ClassName.holdTransition);

    if (this.options.resetHeight) {
      $('body, html, ' + Selector.wrapper).css({
        'height'    : 'auto',
        'min-height': '100%'
      });
    }

    if (!this.bindedResize) {
      $(window).resize(function () {
        this.fix();
        this.fixSidebar();

        $(Selector.logo + ', ' + Selector.sidebar).one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
          this.fix();
          this.fixSidebar();
        }.bind(this));
      }.bind(this));

      this.bindedResize = true;
    }

    $(Selector.sidebarMenu).on('expanded.tree', function () {
      this.fix();
      this.fixSidebar();
    }.bind(this));

    $(Selector.sidebarMenu).on('collapsed.tree', function () {
      this.fix();
      this.fixSidebar();
    }.bind(this));
  };

  Layout.prototype.fix = function () {
    // Remove overflow from .wrapper if layout-boxed exists
    $(Selector.layoutBoxed + ' > ' + Selector.wrapper).css('overflow', 'hidden');

    // Get window height and the wrapper height
    var footerHeight = $(Selector.mainFooter).outerHeight() || 0;
    var headerHeight  = $(Selector.mainHeader).outerHeight() || 0;
    var neg           = headerHeight + footerHeight;
    var windowHeight  = $(window).height();
    var sidebarHeight = $(Selector.sidebar).height() || 0;

    // Set the min-height of the content and sidebar based on
    // the height of the document.
    if ($('body').hasClass(ClassName.fixed)) {
      $(Selector.contentWrapper).css('min-height', windowHeight - footerHeight);
    } else {
      var postSetHeight;

      if (windowHeight >= sidebarHeight + headerHeight) {
        $(Selector.contentWrapper).css('min-height', windowHeight - neg);
        postSetHeight = windowHeight - neg;
      } else {
        $(Selector.contentWrapper).css('min-height', sidebarHeight);
        postSetHeight = sidebarHeight;
      }

      // Fix for the control sidebar height
      var $controlSidebar = $(Selector.controlSidebar);
      if (typeof $controlSidebar !== 'undefined') {
        if ($controlSidebar.height() > postSetHeight)
          $(Selector.contentWrapper).css('min-height', $controlSidebar.height());
      }
    }
  };

  Layout.prototype.fixSidebar = function () {
    // Make sure the body tag has the .fixed class
    if (!$('body').hasClass(ClassName.fixed)) {
      if (typeof $.fn.slimScroll !== 'undefined') {
        $(Selector.sidebar).slimScroll({ destroy: true }).height('auto');
      }
      return;
    }

    // Enable slimscroll for fixed layout
    if (this.options.slimscroll) {
      if (typeof $.fn.slimScroll !== 'undefined') {
        // Destroy if it exists
        // $(Selector.sidebar).slimScroll({ destroy: true }).height('auto')

        // Add slimscroll
        $(Selector.sidebar).slimScroll({
          height: ($(window).height() - $(Selector.mainHeader).height()) + 'px'
        });
      }
    }
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option === 'object' && option);
        $this.data(DataKey, (data = new Layout(options)));
      }

      if (typeof option === 'string') {
        if (typeof data[option] === 'undefined') {
          throw new Error('No method named ' + option);
        }
        data[option]();
      }
    });
  }

  var old = $.fn.layout;

  $.fn.layout            = Plugin;
  $.fn.layout.Constuctor = Layout;

  // No conflict mode
  // ================
  $.fn.layout.noConflict = function () {
    $.fn.layout = old;
    return this;
  };

  // Layout DATA-API
  // ===============
  $(window).on('load', function () {
    Plugin.call($('body'));
  });
}(jQuery);


/* PushMenu()
 * ==========
 * Adds the push menu functionality to the sidebar.
 *
 * @usage: $('.btn').pushMenu(options)
 *          or add [data-toggle="push-menu"] to any button
 *          Pass any option as data-option="value"
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.pushmenu';

  var Default = {
    collapseScreenSize   : 767,
    expandOnHover        : false,
    expandTransitionDelay: 200
  };

  var Selector = {
    collapsed     : '.sidebar-collapse',
    open          : '.sidebar-open',
    mainSidebar   : '.main-sidebar',
    contentWrapper: '.content-wrapper',
    searchInput   : '.sidebar-form .form-control',
    button        : '[data-toggle="push-menu"]',
    mini          : '.sidebar-mini',
    expanded      : '.sidebar-expanded-on-hover',
    layoutFixed   : '.fixed'
  };

  var ClassName = {
    collapsed    : 'sidebar-collapse',
    open         : 'sidebar-open',
    mini         : 'sidebar-mini',
    expanded     : 'sidebar-expanded-on-hover',
    expandFeature: 'sidebar-mini-expand-feature',
    layoutFixed  : 'fixed'
  };

  var Event = {
    expanded : 'expanded.pushMenu',
    collapsed: 'collapsed.pushMenu'
  };

  // PushMenu Class Definition
  // =========================
  var PushMenu = function (options) {
    this.options = options;
    this.init();
  };

  PushMenu.prototype.init = function () {
    if (this.options.expandOnHover
      || ($('body').is(Selector.mini + Selector.layoutFixed))) {
      this.expandOnHover();
      $('body').addClass(ClassName.expandFeature);
    }

    $(Selector.contentWrapper).click(function () {
      // Enable hide menu when clicking on the content-wrapper on small screens
      if ($(window).width() <= this.options.collapseScreenSize && $('body').hasClass(ClassName.open)) {
        this.close();
      }
    }.bind(this));

    // __Fix for android devices
    $(Selector.searchInput).click(function (e) {
      e.stopPropagation();
    });
  };

  PushMenu.prototype.toggle = function () {
    var windowWidth = $(window).width();
    var isOpen      = !$('body').hasClass(ClassName.collapsed);

    if (windowWidth <= this.options.collapseScreenSize) {
      isOpen = $('body').hasClass(ClassName.open);
    }

    if (!isOpen) {
      this.open();
    } else {
      this.close();
    }
  };

  PushMenu.prototype.open = function () {
    var windowWidth = $(window).width();

    if (windowWidth > this.options.collapseScreenSize) {
      $('body').removeClass(ClassName.collapsed)
        .trigger($.Event(Event.expanded));
    }
    else {
      $('body').addClass(ClassName.open)
        .trigger($.Event(Event.expanded));
    }
  };

  PushMenu.prototype.close = function () {
    var windowWidth = $(window).width();
    if (windowWidth > this.options.collapseScreenSize) {
      $('body').addClass(ClassName.collapsed)
        .trigger($.Event(Event.collapsed));
    } else {
      $('body').removeClass(ClassName.open + ' ' + ClassName.collapsed)
        .trigger($.Event(Event.collapsed));
    }
  };

  PushMenu.prototype.expandOnHover = function () {
    $(Selector.mainSidebar).hover(function () {
      if ($('body').is(Selector.mini + Selector.collapsed)
        && $(window).width() > this.options.collapseScreenSize) {
        this.expand();
      }
    }.bind(this), function () {
      if ($('body').is(Selector.expanded)) {
        this.collapse();
      }
    }.bind(this));
  };

  PushMenu.prototype.expand = function () {
    setTimeout(function () {
      $('body').removeClass(ClassName.collapsed)
        .addClass(ClassName.expanded);
    }, this.options.expandTransitionDelay);
  };

  PushMenu.prototype.collapse = function () {
    setTimeout(function () {
      $('body').removeClass(ClassName.expanded)
        .addClass(ClassName.collapsed);
    }, this.options.expandTransitionDelay);
  };

  // PushMenu Plugin Definition
  // ==========================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, (data = new PushMenu(options)));
      }

      if (option === 'toggle') data.toggle();
    });
  }

  var old = $.fn.pushMenu;

  $.fn.pushMenu             = Plugin;
  $.fn.pushMenu.Constructor = PushMenu;

  // No Conflict Mode
  // ================
  $.fn.pushMenu.noConflict = function () {
    $.fn.pushMenu = old;
    return this;
  };

  // Data API
  // ========
  $(document).on('click', Selector.button, function (e) {
    e.preventDefault();
    Plugin.call($(this), 'toggle');
  });
  $(window).on('load', function () {
    Plugin.call($(Selector.button));
  });
}(jQuery);


/* TodoList()
 * =========
 * Converts a list into a todoList.
 *
 * @Usage: $('.my-list').todoList(options)
 *         or add [data-widget="todo-list"] to the ul element
 *         Pass any option as data-option="value"
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.todolist';

  var Default = {
    onCheck  : function (item) {
      return item;
    },
    onUnCheck: function (item) {
      return item;
    }
  };

  var Selector = {
    data: '[data-widget="todo-list"]'
  };

  var ClassName = {
    done: 'done'
  };

  // TodoList Class Definition
  // =========================
  var TodoList = function (element, options) {
    this.element = element;
    this.options = options;

    this._setUpListeners();
  };

  TodoList.prototype.toggle = function (item) {
    item.parents(Selector.li).first().toggleClass(ClassName.done);
    if (!item.prop('checked')) {
      this.unCheck(item);
      return;
    }

    this.check(item);
  };

  TodoList.prototype.check = function (item) {
    this.options.onCheck.call(item);
  };

  TodoList.prototype.unCheck = function (item) {
    this.options.onUnCheck.call(item);
  };

  // Private

  TodoList.prototype._setUpListeners = function () {
    var that = this;
    $(this.element).on('change ifChanged', 'input:checkbox', function () {
      that.toggle($(this));
    });
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, (data = new TodoList($this, options)));
      }

      if (typeof data == 'string') {
        if (typeof data[option] == 'undefined') {
          throw new Error('No method named ' + option);
        }
        data[option]();
      }
    });
  }

  var old = $.fn.todoList;

  $.fn.todoList             = Plugin;
  $.fn.todoList.Constructor = TodoList;

  // No Conflict Mode
  // ================
  $.fn.todoList.noConflict = function () {
    $.fn.todoList = old;
    return this;
  };

  // TodoList Data API
  // =================
  $(window).on('load', function () {
    $(Selector.data).each(function () {
      Plugin.call($(this));
    });
  });

}(jQuery);


/* Tree()
 * ======
 * Converts a nested list into a multilevel
 * tree view menu.
 *
 * @Usage: $('.my-menu').tree(options)
 *         or add [data-widget="tree"] to the ul element
 *         Pass any option as data-option="value"
 */
+function ($) {
  'use strict';

  var DataKey = 'lte.tree';

  var Default = {
    animationSpeed: 500,
    accordion     : true,
    followLink    : false,
    trigger       : '.treeview a'
  };

  var Selector = {
    tree        : '.tree',
    treeview    : '.treeview',
    treeviewMenu: '.treeview-menu',
    open        : '.menu-open, .active',
    li          : 'li',
    data        : '[data-widget="tree"]',
    active      : '.active'
  };

  var ClassName = {
    open: 'menu-open',
    tree: 'tree'
  };

  var Event = {
    collapsed: 'collapsed.tree',
    expanded : 'expanded.tree'
  };

  // Tree Class Definition
  // =====================
  var Tree = function (element, options) {
    this.element = element;
    this.options = options;

    $(this.element).addClass(ClassName.tree);

    $(Selector.treeview + Selector.active, this.element).addClass(ClassName.open);

    this._setUpListeners();
  };

  Tree.prototype.toggle = function (link, event) {
    var treeviewMenu = link.next(Selector.treeviewMenu);
    var parentLi     = link.parent();
    var isOpen       = parentLi.hasClass(ClassName.open);

    if (!parentLi.is(Selector.treeview)) {
      return;
    }

    if (!this.options.followLink || link.attr('href') === '#') {
      event.preventDefault();
    }

    if (isOpen) {
      this.collapse(treeviewMenu, parentLi);
    } else {
      this.expand(treeviewMenu, parentLi);
    }
  };

  Tree.prototype.expand = function (tree, parent) {
    var expandedEvent = $.Event(Event.expanded);

    if (this.options.accordion) {
      var openMenuLi = parent.siblings(Selector.open);
      var openTree   = openMenuLi.children(Selector.treeviewMenu);
      this.collapse(openTree, openMenuLi);
    }

    parent.addClass(ClassName.open);
    tree.slideDown(this.options.animationSpeed, function () {
      $(this.element).trigger(expandedEvent);
    }.bind(this));
  };

  Tree.prototype.collapse = function (tree, parentLi) {
    var collapsedEvent = $.Event(Event.collapsed);

    //tree.find(Selector.open).removeClass(ClassName.open);
    parentLi.removeClass(ClassName.open);
    tree.slideUp(this.options.animationSpeed, function () {
      //tree.find(Selector.open + ' > ' + Selector.treeview).slideUp();
      $(this.element).trigger(collapsedEvent);
    }.bind(this));
  };

  // Private

  Tree.prototype._setUpListeners = function () {
    var that = this;

    $(this.element).on('click', this.options.trigger, function (event) {
      that.toggle($(this), event);
    });
  };

  // Plugin Definition
  // =================
  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data  = $this.data(DataKey);

      if (!data) {
        var options = $.extend({}, Default, $this.data(), typeof option == 'object' && option);
        $this.data(DataKey, new Tree($this, options));
      }
    });
  }

  var old = $.fn.tree;

  $.fn.tree             = Plugin;
  $.fn.tree.Constructor = Tree;

  // No Conflict Mode
  // ================
  $.fn.tree.noConflict = function () {
    $.fn.tree = old;
    return this;
  };

  // Tree Data API
  // =============
  $(window).on('load', function () {
    $(Selector.data).each(function () {
      Plugin.call($(this));
    });
  });

}(jQuery);
